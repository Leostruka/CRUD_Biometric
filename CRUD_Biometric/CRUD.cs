using CRUD_Biometric.Model;
using CRUD_User.View;
using CRUD_Biometric.DataAccess;
using NITGEN.SDK.NBioBSP;
using System.Data;
using CRUD_Biometric.Properties;
using System.Management;
using System.ComponentModel;

namespace CRUD_Biometric
{
    public partial class CRUD : Form
    {
        #region ------------------------------Variables and Objects-------------------------------------------------

        // NBioAPI objects
        NBioAPI m_NBioAPI;
        NBioAPI.Export m_Export;
        NBioAPI.IndexSearch m_IndexSearch;

        // FIR and Audit objects
        NBioAPI.Type.HFIR hActivatedFIR;
        UserModel.User user;
        FIRModel.FIR fir;
        AuditModel.Audit audit;
        // FIR and Audit objects for replace
        FIRModel.FIR replaseFir;
        AuditModel.Audit replaseAudit;

        // DataTables for user and FIR data
        DataTable dt_user_fir;

        // Variables for sample navigation
        int firstSample = 0;
        int currentSampleNumber = 0;

        // WMI query to monitor for device arrival events
        ManagementEventWatcher arrivalWatcher;
        // WMI query to monitor for device removal events
        ManagementEventWatcher removalWatcher;

        // Device information
        short[] deviceID;
        // Get device information
        NBioAPI.Type.DEVICE_INFO_EX[] deviceInfoEx;
        int currentDeviceID;
        bool isFingerPlaced;

        SQL sql;
        private Label lb_dbStatus;

        #endregion

        #region ------------------------------Methods For Forms-----------------------------------------------------

        public CRUD()
        {
            InitializeComponent();

            // Add database status label
            lb_dbStatus = new Label();
            lb_dbStatus.AutoSize = true;
            lb_dbStatus.Location = new Point(tx_NBioV.Right + 10, tx_NBioV.Top);
            lb_dbStatus.Text = "DB: Checking...";
            lb_dbStatus.Font = new Font("Montserrat", 8, FontStyle.Regular);
            this.Controls.Add(lb_dbStatus);

            // Initialize NBioAPI
            m_NBioAPI = new NBioAPI();
            // Initialize IndexSearch
            m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            // Initialize Export for image conversion and extraction
            m_Export = new NBioAPI.Export(m_NBioAPI);
            uint ret = m_IndexSearch.InitEngine();
            if (ret != NBioAPI.Error.NONE)
            {
                ErrorMsg(ret);
                this.Close();
            }
            // Get version of NBioAPI
            m_NBioAPI.GetVersion(out NBioAPI.Type.VERSION version);
            tx_NBioV.Text += version.Major + "." + version.Minor;

            // Initialize User, FIR and Audit objects
            user = new UserModel.User();
            fir = new FIRModel.FIR();
            audit = new AuditModel.Audit();
            // Initialize FIR and Audit objects for replace
            replaseFir = new FIRModel.FIR();
            replaseAudit = new AuditModel.Audit();

            // Get Device(s) connected and information
            SearchDevice();

            // Update dg_users
            UpdateDGUsers();

            // Initialize WMI event watchers 
            InitializeWmiWatchers();

            // Initialize background worker for finger check
            InitializeFingerCheckWorkerFunctions();
        }

        #endregion

        #region ------------------------------Methods For Events----------------------------------------------------

        // Initialize WMI event watchers
        private void InitializeWmiWatchers()
        {
            // WMI query to monitor for device arrival events
            string arrivalQuery = "SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2";
            arrivalWatcher = new ManagementEventWatcher(arrivalQuery);
            arrivalWatcher.EventArrived += ArrivalEventArrived;
            arrivalWatcher.Start();

            // WMI query to monitor for device removal events
            string removalQuery = "SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3";
            removalWatcher = new ManagementEventWatcher(removalQuery);
            removalWatcher.EventArrived += RemovalEventArrived;
            removalWatcher.Start();
        }

        private void InitializeFingerCheckWorkerFunctions()
        {
            // Initialize the background worker for finger check
            fingerCheckWorker.DoWork += fingerCheckWorker_DoWork;
            fingerCheckWorker.WorkerSupportsCancellation = true;
        }

        #endregion

        #region ------------------------------Methods Called--------------------------------------------------------

        // Error message
        private void ErrorMsg(uint ret) => MessageBox.Show("Error: " + ret.ToString() + "\n" + NBioAPI.Error.GetErrorDescription(ret), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // Check if the name is valid
        private bool IsValidName(string name)
        {
            // Check if the name is empty
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Check if the name contains any numbers
            if (name.Any(char.IsDigit))
            {
                return false;
            }

            // Check if the name contains any special characters
            if (name.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)))
            {
                return false;
            }

            return true;
        }

        // Convert hexadecimal string to byte array
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        // Get serial number of the device
        public string GetSerialNumber(short deviceID)
        {
            byte[] serial;
            byte[] input = new byte[8];

            m_NBioAPI.OpenDevice(deviceID);
            m_NBioAPI.DeviceIoControl(514, input, out serial);
            m_NBioAPI.CloseDevice(deviceID);

            return BitConverter.ToString(serial);
        }

        // Update dg_users based on database
        private void UpdateDGUsers()
        {
            // Set SQL and get data
            sql = new SQL();
            dt_user_fir = sql.GetDataUserFir();
            
            // Update database status label
            Connection connection = new Connection();
            if (connection.CheckInternetConnection())
            {
                lb_dbStatus.Text = "DB: MySQL (Online)";
                lb_dbStatus.ForeColor = Color.DarkGreen;
            }
            else
            {
                lb_dbStatus.Text = "DB: SQLite (Offline)";
                lb_dbStatus.ForeColor = Color.DarkOrange;
            }

            // Clear existing data in dg_users
            dg_users.Rows.Clear();
            dg_users.Columns.Clear();

            // Add columns to dg_users
            dg_users.Columns.Add("ID", "ID");
            dg_users.Columns.Add("Name", "Name");
            dg_users.Columns.Add("Samples", "Sample amount");

            // Populate dg_users with user information
            foreach (DataRow row in dt_user_fir.Rows)
            {
                int userID = Convert.ToInt32(row["id"]);
                string userName = row["name"]?.ToString() ?? string.Empty;

                // Get the number of samples for the current user ID
                int sampleAmount = dt_user_fir.AsEnumerable().Count(row => Convert.ToInt32(row["id"]) == userID);

                // Check if the user already exists in the dg_users
                bool userExists = false;
                foreach (DataGridViewRow dgRow in dg_users.Rows)
                {
                    if (Convert.ToInt32(dgRow.Cells["ID"].Value) == userID)
                    {
                        userExists = true;
                        break;
                    }
                }

                // Add the user to dg_users if it doesn't exist
                if (!userExists)
                {
                    dg_users.Rows.Add(userID, userName, sampleAmount);

                }
            }

            if (dg_users.Rows.Count > 0)
            {
                bt_modify.Enabled = true;
                bt_remove.Enabled = true;
            }
            else
            {
                bt_modify.Enabled = false;
                bt_remove.Enabled = false;
            }
            UpdateIndexSearch();
        }

        // Update IndexSearchDB
        private void UpdateIndexSearch()
        {
            m_IndexSearch.ClearDB();

            // Add FIR to IndexSearchDB if exists in database
            if (dt_user_fir.Rows.Count > 0)
            {
                NBioAPI.IndexSearch.FP_INFO[] fpinfo;
                for (int i = 0; i < dt_user_fir.Rows.Count; i++)
                {
                    NBioAPI.Type.FIR_TEXTENCODE textFIR = new NBioAPI.Type.FIR_TEXTENCODE();

                    textFIR.TextFIR = dt_user_fir.Rows[i]["hash"].ToString();
                    string id = dt_user_fir.Rows[i]["id"].ToString() + "909" + dt_user_fir.Rows[i]["sample"].ToString();
                    uint ret = m_IndexSearch.AddFIR(textFIR, Convert.ToUInt32(id), out fpinfo);
                    if (ret != NBioAPI.Error.NONE)
                    {
                        ErrorMsg(ret);
                        this.Close();
                    }
                }
            }
        }

        // Update FirstSample based on the database
        public void UpdateFirstSample()
        {
            firstSample = 0;

            for (int i = 0; i < dt_user_fir.Rows.Count; i++)
            {
                DataRow row = dt_user_fir.Rows[i];

                if (Convert.ToInt32(row["id"]) == Convert.ToInt32(tb_userID.Text) && firstSample == 0)
                {
                    firstSample = Convert.ToInt32(row["sample"]);
                }
            }
        }

        // Select FIR based on ID and sample, set image and text
        private void AttSelectFir(int id, int sample)
        {
            AuditModel.Audit selectAudit = new AuditModel.Audit();
            selectAudit.id = id;

            int y = 0;

            for (int i = 0; i < dt_user_fir.Rows.Count; i++)
            {
                if (Convert.ToInt32(dt_user_fir.Rows[i]["id"]) == selectAudit.id)
                {
                    DataTable dt_audit = sql.GetSpecificDataAudit(selectAudit.id);

                    foreach (DataRow row in dt_audit.Rows)
                    {
                        y++;
                        if (Convert.ToInt32(row["sample"]) == sample)
                        {
                            selectAudit.data = StringToByteArray(row["data"].ToString());
                            selectAudit.imageWidth = Convert.ToUInt32(row["imageWidth"]);
                            selectAudit.imageHeight = Convert.ToUInt32(row["imageHeight"]);
                            currentSampleNumber = y;
                        }
                    }

                    pb_selectedFir.Image = ConvertFIRToJpg(selectAudit);
                    pb_selectedFir.Size = new Size(124, 146);
                    tx_selected.Location = new Point(235, 10);

                    tb_userID.Text = selectAudit.id.ToString();

                    if (currentSampleNumber == 1)
                    {
                        bt_returnSample.Enabled = false;
                        bt_nextSample.Enabled = true;
                    }
                    else if (currentSampleNumber == Convert.ToInt32(dg_users.SelectedRows[0].Cells[2].Value + string.Empty))
                    {
                        bt_nextSample.Enabled = false;
                        bt_returnSample.Enabled = true;
                    }
                    else
                    {
                        bt_nextSample.Enabled = true;
                        bt_returnSample.Enabled = true;
                    }

                    tx_sampleCount.Text = currentSampleNumber + "/" + dg_users.SelectedRows[0].Cells[2].Value + string.Empty;

                    break;
                }
            }
        }

        // Update ActivateCapture
        private void AttActivateCapture(NBioAPI.Type.HFIR hFIR, NBioAPI.Type.HFIR hAuditFIR)
        {
            // Add item to pb_ActivatedCapture
            pb_actvatedFir.Image = ConvertFIRToJpg(hAuditFIR);
            pb_actvatedFir.Size = new Size(124, 146);
            tx_actual.Location = new Point(32, 10);
            hActivatedFIR = hFIR;

            NBioAPI.IndexSearch.CALLBACK_INFO_0 cbInfo = new();
            m_IndexSearch.IdentifyData(hActivatedFIR, NBioAPI.Type.FIR_SECURITY_LEVEL.NORMAL, out NBioAPI.IndexSearch.FP_INFO fpInfo, cbInfo);

            if (fpInfo.ID != 0)
            {
                string id = fpInfo.ID.ToString();
                int index = id.IndexOf("909");
                string before909 = id.Substring(0, index);
                string after909 = id.Substring(index + 3);
                fpInfo.ID = Convert.ToUInt32(before909);
                fpInfo.SampleNumber = (byte)Convert.ToUInt32(after909);
                tb_userID.Text = fpInfo.ID.ToString();
                tb_sample.Text = fpInfo.SampleNumber.ToString();
            }

            bt_register.Enabled = true;
        }

        // Convert FIR to JPG image based on hFIR handle and set audit data
        private Image ConvertFIRToJpg(NBioAPI.Type.HFIR hAuditFIR)
        {
            NBioAPI.Export.EXPORT_AUDIT_DATA exportAuditData;
            m_Export.NBioBSPToImage(hAuditFIR, out exportAuditData);

            audit.data = exportAuditData.AuditData[0].Image[0].Data;
            audit.imageHeight = exportAuditData.ImageHeight;
            audit.imageWidth = exportAuditData.ImageWidth;

            uint r = m_NBioAPI.ImgConvRawToJpgBuf(audit.data, audit.imageWidth, audit.imageHeight, 100, out byte[] outbuffer);
            if (r != NBioAPI.Error.NONE)
            {
                ErrorMsg(r);
                return Resources.error_500_page_empty_symbol_260nw_1711106146;
            }

            return Image.FromStream(new MemoryStream(outbuffer));
        }

        // Convert FIR to JPG image based on Audit data
        private Image ConvertFIRToJpg(AuditModel.Audit audit)
        {
            uint r = m_NBioAPI.ImgConvRawToJpgBuf(audit.data, audit.imageWidth, audit.imageHeight, 100, out byte[] outbuffer);
            if (r != NBioAPI.Error.NONE)
            {
                ErrorMsg(r);
                return Resources.error_500_page_empty_symbol_260nw_1711106146;
            }

            return Image.FromStream(new MemoryStream(outbuffer));
        }

        // Convert Verify Fir to Enroll Fir
        public NBioAPI.Type.HFIR ConvertToEnroll(NBioAPI.Type.HFIR verifyFIR)
        {
            NBioAPI.Type.HFIR processedFIR;
            uint result = m_NBioAPI.Process(verifyFIR, out processedFIR);

            if (result != 0)
            {
                throw new Exception($"Failed to process fingerprint. Error code: {result}");
            }

            NBioAPI.Type.HFIR enrollFIR;
            result = m_NBioAPI.CreateTemplate(processedFIR, (NBioAPI.Type.HFIR)null, out enrollFIR, (NBioAPI.Type.FIR_PAYLOAD)null);

            if (result != 0)
            {
                throw new Exception($"Failed to create enroll template. Error code: {result}");
            }

            return enrollFIR;
        }

        #endregion

        #region ------------------------------Methods For USB and Device--------------------------------------------

        // For the usb arrival and removal events
        private void ArrivalEventArrived(object sender, EventArgs e)
        {
            // USB device was plugged in
            SearchDevice();
        }
        private void RemovalEventArrived(object sender, EventArgs e)
        {
            // USB device was removed
            SearchDevice();
        }

        // Handle the DoWork event of the background worker
        private void fingerCheckWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            m_NBioAPI.OpenDevice(deviceID[currentDeviceID]);

            // verify finger check
            int i = 0;

            while (cb_autoOn.Checked)
            {
                // Continuously check if a finger is placed on the device
                m_NBioAPI.CheckFinger(out isFingerPlaced);

                // If a finger is placed on the device, enable the capture button
                if (isFingerPlaced && i == 0)
                {
                    i++;
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            if (tc_modify.Visible && tc_modify.SelectedTab == tp_sample)
                            {
                                bt_sampleReplace_Click(sender, e);
                            }
                            else if (!tc_modify.Visible)
                            {
                                bt_capture_Click(sender, e);
                            }
                        }));
                    }
                    else
                    {
                        if (tc_modify.Visible && tc_modify.SelectedTab == tp_sample)
                        {
                            bt_sampleReplace_Click(sender, e);
                        }
                        else if (!tc_modify.Visible)
                        {
                            bt_capture_Click(sender, e);
                        }
                    }
                }
                else if (!isFingerPlaced && i != 0)
                {
                    i = 0;
                }

                // Add a delay to avoid continuous checking and reduce CPU usage
                Thread.Sleep(100);
            }
            m_NBioAPI.CloseDevice(deviceID[currentDeviceID]);
        }

        // Search for the device
        private void SearchDevice()
        {
            // Get the number of devices connected and some information
            m_NBioAPI.EnumerateDevice(out uint numDevices, out deviceID, out deviceInfoEx);

            // Clear the flow layout panel
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    flp_devices.Controls.Clear();
                }));
            }
            else
            {
                flp_devices.Controls.Clear();
            }

            // if no any device connected
            if (numDevices == 0)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        bt_capture.Enabled = false;
                        bt_sampleReplace.Enabled = false;
                        pn_deviceInf.Visible = false;
                    }));
                }
                else
                {
                    bt_capture.Enabled = false;
                    bt_sampleReplace.Enabled = false;
                    pn_deviceInf.Visible = false;
                }

                currentDeviceID = -1;
                MessageBox.Show("No device connected!\nCapture functions are disabled.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        bt_capture.Enabled = true;
                        bt_sampleReplace.Enabled = true;
                        pn_deviceInf.Visible = true;
                    }));
                }
                else
                {
                    bt_capture.Enabled = true;
                    bt_sampleReplace.Enabled = true;
                    pn_deviceInf.Visible = true;
                }

                // Add a new button for each device
                for (int i = 0; i < numDevices; i++)
                {
                    Button bt_device = new Button();
                    bt_device.Name = "hamister" + i;
                    bt_device.Size = new Size(34, 28);
                    bt_device.FlatStyle = FlatStyle.Flat;
                    bt_device.Anchor = AnchorStyles.Right;
                    bt_device.FlatAppearance.BorderSize = 0;
                    bt_device.BackgroundImageLayout = ImageLayout.Zoom;
                    bt_device.FlatAppearance.MouseOverBackColor = Color.LightSteelBlue;
                    bt_device.Font = new Font("Montserrat", 5, FontStyle.Bold);
                    bt_device.TextAlign = ContentAlignment.TopLeft;

                    if (deviceInfoEx[i].Name == "FDU01" || deviceInfoEx[i].Name == "FDU04" || deviceInfoEx[i].Name == "FDU06")
                    {
                        bt_device.Text = (i + 1).ToString();
                        bt_device.Click += new EventHandler(bt_device_Click);
                        bt_device.BackgroundImage = Resources.H_DX;

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() =>
                            {
                                flp_devices.Controls.Add(bt_device);
                            }));
                        }
                        else
                        {
                            flp_devices.Controls.Add(bt_device);
                        }
                    }
                    else if (deviceInfoEx[i].Name == "FDU11" || deviceInfoEx[i].Name == "FDU14" || deviceInfoEx[i].Name == "FDU06M" || deviceInfoEx[i].Name == "FDU06S")
                    {
                        bt_device.Text = (i + 1).ToString();
                        bt_device.Click += new EventHandler(bt_device_Click);
                        bt_device.BackgroundImage = Resources.H_3;

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() =>
                            {
                                flp_devices.Controls.Add(bt_device);
                            }));
                        }
                        else
                        {
                            flp_devices.Controls.Add(bt_device);
                        }
                    }

                    if (i == 0)
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() =>
                            {
                                bt_device_Click(bt_device, new EventArgs());
                            }));
                        }
                        else
                        {
                            bt_device_Click(bt_device, new EventArgs());
                        }
                    }
                }
            }
        }

        // Add a new event handler for each device button and add buttons
        private void bt_device_Click(object sender, EventArgs e)
        {
            // Uncheck the autoOn checkbox
            cb_autoOn.Checked = false;

            // Get the device ID
            Button clickedButton = (Button)sender;
            currentDeviceID = int.Parse(clickedButton.Name.Substring(8));

            // Set the device name and serial number
            if (deviceInfoEx[currentDeviceID].Name == "FDU01" || deviceInfoEx[currentDeviceID].Name == "FDU04" || deviceInfoEx[currentDeviceID].Name == "FDU06")
            {
                tb_deviceName.Text = "Hamster DX";
            }
            else
            {
                tb_deviceName.Text = "Hamster III";
            }

            // Get the device ID
            string serialNumber = GetSerialNumber(deviceID[currentDeviceID]);
            tb_serialN.Text = serialNumber;

            // Disable all buttons in flp_devices
            foreach (Button button in flp_devices.Controls)
            {
                button.Enabled = true;
                button.BackColor = Color.Empty;
            }

            // Disable the pressed button
            clickedButton.Enabled = false;
            clickedButton.BackColor = Color.LightSkyBlue;

            bt_capture.Focus();
        }

        // cb_autoOn value changed
        private void cb_autoOn_CheckedChanged(object sender, EventArgs e)
        {
            // Enable event to track if autoOn is modified to true
            if (cb_autoOn.Checked)
            {
                // Start the background worker to perform the finger check operation
                fingerCheckWorker.RunWorkerAsync();

                bt_capture.Enabled = false;
                bt_sampleReplace.Enabled = false;
            }
            // Disable event to track if autoOn is modified to true
            else
            {
                // Cancel the background worker if it is running
                if (fingerCheckWorker.IsBusy)
                {
                    fingerCheckWorker.CancelAsync();

                    if (m_NBioAPI.GetOpenedDeviceID() == deviceID[currentDeviceID])
                    {
                        m_NBioAPI.CloseDevice(deviceID[currentDeviceID]);
                    }
                }

                bt_capture.Enabled = true;
                bt_sampleReplace.Enabled = true;
            }
        }

        #endregion

        #region ------------------------------Methods For Capture, Register and Delete------------------------------

        // Capture FIR
        private void bt_capture_Click(object sender, EventArgs e)
        {
            NBioAPI.Type.HFIR hNewFIR;

            NBioAPI.Type.HFIR hAuditFIR = new NBioAPI.Type.HFIR();

            // Capture FIR
            if (cb_autoOn.Checked)
            {
                NBioAPI.Type.WINDOW_OPTION windowOption = new NBioAPI.Type.WINDOW_OPTION();
                windowOption.WindowStyle = NBioAPI.Type.WINDOW_STYLE.INVISIBLE;
                // Capture FIR
                uint ret = m_NBioAPI.Capture(NBioAPI.Type.FIR_PURPOSE.VERIFY, out hNewFIR, NBioAPI.Type.TIMEOUT.DEFAULT, hAuditFIR, windowOption);
                if (ret != NBioAPI.Error.NONE)
                {
                    ErrorMsg(ret);
                    return;
                }
            }
            else
            {
                m_NBioAPI.OpenDevice(deviceID[currentDeviceID]);
                uint ret = m_NBioAPI.Capture(NBioAPI.Type.FIR_PURPOSE.VERIFY, out hNewFIR, NBioAPI.Type.TIMEOUT.DEFAULT, hAuditFIR, null);
                if (ret != NBioAPI.Error.NONE)
                {
                    ErrorMsg(ret);
                    m_NBioAPI.CloseDevice(deviceID[currentDeviceID]);
                    return;
                }
                m_NBioAPI.CloseDevice(deviceID[currentDeviceID]);
            }


            // Clear pb_actvaredFir
            pb_actvatedFir.Image = null;

            // Activate FIR
            AttActivateCapture(hNewFIR, hAuditFIR);
        }

        // Register activated FIR, if new user, register user and second FIR
        private void bt_register_Click(object sender, EventArgs e)
        {
            if (hActivatedFIR == null)
            {
                MessageBox.Show("Please, capture a FIR first!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NBioAPI.Type.HFIR hCapturedFIR;
            NBioAPI.Type.HFIR hAuditFIR = new NBioAPI.Type.HFIR();

            // Verify if ID is valid
            try
            {
                int test = Convert.ToInt32(tb_userID.Text, 10);
                if (test == 0)
                {
                    throw (new Exception());
                }
            }
            catch
            {
                MessageBox.Show("Invalid ID!\n User ID must be have numeric type and greated than 0.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get ID
            uint userID = Convert.ToUInt32(tb_userID.Text, 10);

            NBioAPI.IndexSearch.CALLBACK_INFO_0 cbInfo = new();

            m_IndexSearch.IdentifyData(hActivatedFIR, NBioAPI.Type.FIR_SECURITY_LEVEL.NORMAL, out NBioAPI.IndexSearch.FP_INFO fpInfo, cbInfo);

            // Convert the FIR to enroll template
            NBioAPI.Type.HFIR enrollFIR = ConvertToEnroll(hActivatedFIR);

            // Check if the finger already exists
            if (fpInfo.ID != 0)
            {
                string id = fpInfo.ID.ToString();
                int index = id.IndexOf("909");
                string before909 = id.Substring(0, index);
                string after909 = id.Substring(index + 3);
                fpInfo.ID = Convert.ToUInt32(before909);
                fpInfo.SampleNumber = (byte)Convert.ToUInt32(after909);

                string nome = string.Empty;
                foreach (DataRow row in dt_user_fir.Rows)
                {
                    if (Convert.ToInt32(row["id"]) == userID)
                    {
                        nome = row["name"].ToString();
                        break;
                    }
                }

                if (DialogResult.Yes == MessageBox.Show("This finger belongs to the " + nome + "\nUser ID: " + (int)fpInfo.ID + "\nSample: " + fpInfo.SampleNumber + "\nRegistry anyway?", "Existing Finger!", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    m_NBioAPI.GetTextFIRFromHandle(enrollFIR, out NBioAPI.Type.FIR_TEXTENCODE newTextFIR, true);
                    fir.id = (int)fpInfo.ID;
                    fir.hash = newTextFIR.TextFIR;
                    fir.sample = 1;
                    audit.id = (int)fpInfo.ID;
                    foreach (DataRow row in dt_user_fir.Rows)
                    {
                        if (Convert.ToInt32(row["id"]) == fir.id)
                        {
                            if (Convert.ToInt32(row["sample"]) <= fir.sample)
                            {
                                fir.sample += 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    audit.sample = fir.sample;
                    sql.InsertDataFir(fir); // Register FIR
                    sql.InsertDataAudit(audit); // Register Audit
                    MessageBox.Show("User ID: " + (int)fpInfo.ID + "\nName: " + nome + "\nnew sample registered!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    UpdateDGUsers();

                    pb_actvatedFir.Image = null;

                    tb_userID.Text = fir.id.ToString();
                    tb_sample.Text = fir.sample.ToString();

                    bt_register.Enabled = false;
                    return;
                }
                else
                {
                    return;
                }
            }
            // Check if the user already exists
            else
            {
                foreach (DataRow row in dt_user_fir.Rows)
                {
                    if (Convert.ToInt32(row["id"]) == userID)
                    {
                        if (DialogResult.Yes == MessageBox.Show("The user ID: " + (int)userID + " already exists!\nName: " + row["name"] + "\nRegistry anyway?", "Existing User!", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            m_NBioAPI.GetTextFIRFromHandle(enrollFIR, out NBioAPI.Type.FIR_TEXTENCODE newTextFIR, true);
                            fir.id = (int)userID;
                            fir.hash = newTextFIR.TextFIR;
                            fir.sample = 1;
                            audit.id = (int)userID;
                            foreach (DataRow row2 in dt_user_fir.Rows)
                            {
                                if (Convert.ToInt32(row2["id"]) == fir.id)
                                {
                                    if (Convert.ToInt32(row2["sample"]) <= fir.sample)
                                    {
                                        fir.sample += 1;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            audit.sample = fir.sample;
                            sql.InsertDataFir(fir); // Register FIR
                            sql.InsertDataAudit(audit); // Register Audit
                            MessageBox.Show("ID: " + row["id"] + "\nName: " + row["name"] + "\nnew sample registered!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            UpdateDGUsers();

                            pb_actvatedFir.Image = null;

                            tb_userID.Text = fir.id.ToString();
                            tb_sample.Text = fir.sample.ToString();

                            bt_register.Enabled = false;
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }

            // Register new user and FIR
            // Capture FIR2
            MessageBox.Show("Please, put the same finger of the capture!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Cancel the background worker if it is running
            if (cb_autoOn.Checked)
            {
                if (fingerCheckWorker.IsBusy)
                {
                    fingerCheckWorker.CancelAsync();

                    if (m_NBioAPI.GetOpenedDeviceID() == deviceID[currentDeviceID])
                    {
                        m_NBioAPI.CloseDevice(deviceID[currentDeviceID]);
                    }
                }
            }

            m_NBioAPI.OpenDevice(deviceID[currentDeviceID]);
            uint ret = m_NBioAPI.Capture(NBioAPI.Type.FIR_PURPOSE.VERIFY, out hCapturedFIR, NBioAPI.Type.TIMEOUT.DEFAULT, hAuditFIR, null);
            if (ret != NBioAPI.Error.NONE)
            {
                ErrorMsg(ret);
                m_NBioAPI.CloseDevice(deviceID[currentDeviceID]);
                return;
            }
            m_NBioAPI.CloseDevice(deviceID[currentDeviceID]);

            // Convert the FIR to enroll template
            NBioAPI.Type.HFIR enrollCapturedFIR = ConvertToEnroll(hCapturedFIR);

            // Get FIR2 and compare with activated FIR
            m_NBioAPI.GetTextFIRFromHandle(enrollFIR, out NBioAPI.Type.FIR_TEXTENCODE textFIR, true);
            m_NBioAPI.GetTextFIRFromHandle(enrollCapturedFIR, out NBioAPI.Type.FIR_TEXTENCODE textFIR2, true);
            m_NBioAPI.VerifyMatch(textFIR, textFIR2, out bool result, null);

            // Check if FIRs match
            if (result)
            {
                // Set UserID and FIRID
                user.id = (int)userID;

                // Open UserInformationView
                UserInformationView userInformationView = new UserInformationView();
                while (!IsValidName(userInformationView.GetName()))
                {
                    userInformationView.SetName("");
                    userInformationView.ShowDialog();
                    if (userInformationView.GetName() == "Cancel")
                    {
                        return;
                    }
                    else if (!IsValidName(userInformationView.GetName()))
                    {
                        MessageBox.Show("Please enter a valid name.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                // Get user information
                user.name = userInformationView.GetName();

                // Register user in to database
                sql.InsertDataUser(user);

                // Set Fir hash and sample
                fir.id = (int)userID;
                fir.hash = textFIR.TextFIR;
                fir.sample = 1;
                sql.InsertDataFir(fir); // Register FIR

                audit.id = (int)userID;
                audit.sample = fir.sample;
                sql.InsertDataAudit(audit); // Register 

                // Set Fir hash and sample 2
                fir.hash = textFIR2.TextFIR;
                fir.sample += 1;
                sql.InsertDataFir(fir); // Register FIR2

                ConvertFIRToJpg(hAuditFIR);
                audit.id = (int)userID;
                audit.sample = fir.sample;
                sql.InsertDataAudit(audit); // Register 

                tb_userID.Text = fir.id.ToString();
                tb_sample.Text = fir.sample.ToString();

                MessageBox.Show("User ID: " + userID.ToString() + "\nName: " + user.name + "\nregistered!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pb_actvatedFir.Image = null;

                bt_register.Enabled = false;
            }
            else
            {
                MessageBox.Show("FIRs do not match!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Update dg_users
            UpdateDGUsers();
            tb_userID.Text = userID.ToString();
            tb_sample.Text = fir.sample.ToString();

            if (cb_autoOn.Checked)
            {
                fingerCheckWorker.RunWorkerAsync();
            }
        }

        // Remove sample, if the last sample, remove user
        private void bt_remove_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(dg_users.SelectedRows[0].Cells[2].Value + string.Empty) == 1)
            {
                if (DialogResult.Yes == MessageBox.Show("Do you want to delete the user?", "Delete user", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    sql.DeleteDataFirAudit(Convert.ToInt32(tb_userID.Text), Convert.ToInt32(tb_sample.Text));
                    sql.DeleteDataUser(Convert.ToInt32(tb_userID.Text));
                    UpdateDGUsers();
                }
            }
            else if (DialogResult.Yes == MessageBox.Show("Do you want to delete the sample?", "Delete sample", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                sql.DeleteDataFirAudit(Convert.ToInt32(tb_userID.Text), Convert.ToInt32(tb_sample.Text));
                foreach (DataRow row in dt_user_fir.Rows)
                {
                    if (Convert.ToInt32(row["id"]) == Convert.ToInt32(tb_userID.Text))
                    {
                        UpdateFirstSample();
                        if (firstSample != Convert.ToInt32(tb_sample.Text))
                        {
                            bt_returnSample_Click(sender, e);
                        }
                        else
                        {
                            bt_nextSample_Click(sender, e);
                        }

                        string id = tb_userID.Text;
                        string sample = tb_sample.Text;

                        UpdateDGUsers();

                        tb_userID.Text = id;
                        tb_sample.Text = sample;

                        break;
                    }
                }
            }
        }

        #endregion

        #region ------------------------------Methods To Select User and Sample-------------------------------------

        // Get selected user from dg_users
        private void dg_users_SelectionChanged(object sender, EventArgs e)
        {
            if (dg_users.SelectedRows.Count > 0)
            {
                AuditModel.Audit selectedAudit = new AuditModel.Audit();
                selectedAudit.id = Convert.ToInt32(dg_users.SelectedRows[0].Cells[0].Value + string.Empty);
                tb_userID.Text = selectedAudit.id.ToString();

                foreach (DataRow row in dt_user_fir.Rows)
                {
                    if (Convert.ToInt32(row["id"]) == selectedAudit.id)
                    {
                        tb_sample.Text = row["sample"].ToString();
                        if (Convert.ToInt32(dg_users.SelectedRows[0].Cells[2].Value + string.Empty) > 1)
                        {
                            bt_returnSample.Enabled = false;
                            bt_nextSample.Enabled = true;
                            tx_sampleCount.Text = "1/" + dg_users.SelectedRows[0].Cells[2].Value + string.Empty;
                        }
                        else if (Convert.ToInt32(dg_users.SelectedRows[0].Cells[2].Value + string.Empty) == 1)
                        {
                            bt_returnSample.Enabled = false;
                            bt_nextSample.Enabled = false;
                            tx_sampleCount.Text = "1/1";
                        }
                        currentSampleNumber = 1;
                        AttSelectFir(Convert.ToInt32(tb_userID.Text), Convert.ToInt32(tb_sample.Text));
                        break;
                    }
                }
            }
        }

        // Search for user ID in dg_users when typing in tb_userID
        private void tb_userID_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dg_users.Rows.Count; i++)
            {
                if (dg_users.Rows[i].Cells[0].Value.ToString() == tb_userID.Text)
                {
                    dg_users.Rows[i].Selected = true;
                    break;
                }
            }
        }

        // Get selected sample and update selected FIR img
        private void tb_sample_TextChanged(object sender, EventArgs e)
        {
            tb_sample.Focus();

            int index = tx_selectedIDSample.Text.IndexOf(" - ");
            tx_selectedIDSample.Text = tx_selectedIDSample.Text.ToString().Substring(0, index) + " - " + tb_sample.Text;

            AttSelectFir(int.Parse(tb_userID.Text), int.Parse(tb_sample.Text));
        }

        // Return to previous sample
        private void bt_returnSample_Click(object sender, EventArgs e)
        {
            int currentSample = int.Parse(tb_sample.Text) - 1;
            int previousSample = 0;

            for (int i = 0; i < dt_user_fir.Rows.Count; i++)
            {
                DataRow row = dt_user_fir.Rows[i];
                if (Convert.ToInt32(row["id"]) == Convert.ToInt32(tb_userID.Text) && Convert.ToInt32(row["sample"]) <= currentSample)
                {
                    previousSample = Convert.ToInt32(row["sample"]);
                }
            }
            currentSample = previousSample;
            tb_sample.Text = currentSample.ToString();
        }

        // Go to next sample
        private void bt_nextSample_Click(object sender, EventArgs e)
        {
            int currentSample = int.Parse(tb_sample.Text) + 1;

            foreach (DataRow row in dt_user_fir.Rows)
            {
                if (Convert.ToInt32(row["id"]) == Convert.ToInt32(tb_userID.Text) && Convert.ToInt32(row["sample"]) >= currentSample)
                {
                    currentSample = Convert.ToInt32(row["sample"]);
                    break;
                }
            }
            tb_sample.Text = currentSample.ToString();
        }

        #endregion

        #region ------------------------------Methods For Modify----------------------------------------------------

        // Modify user name or selected sample
        private void bt_modify_Click(object sender, EventArgs e)
        {
            if (!tc_modify.Visible)
            {
                tx_selectedID.Text = dg_users.SelectedRows[0].Cells[1].Value + " : " + dg_users.SelectedRows[0].Cells[0].Value + string.Empty;
                tx_selectedIDSample.Text = dg_users.SelectedRows[0].Cells[1].Value + string.Empty + " : " + dg_users.SelectedRows[0].Cells[0].Value + string.Empty + " - " + tb_sample.Text;

                user.id = Convert.ToInt32(dg_users.SelectedRows[0].Cells[0].Value + string.Empty);
                user.name = dg_users.SelectedRows[0].Cells[1].Value + string.Empty;

                tc_modify.Visible = true;
                tb_alterName.Text = "";
                tc_modify.SelectedTab = tp_user;
                dg_users.Enabled = false;
                dg_users.BackgroundColor = Color.Gray;
                dg_users.DefaultCellStyle.SelectionBackColor = Color.Gray;
                tb_userID.Enabled = false;
                bt_modify.Text = "Cancel";

                bt_capture.Enabled = false;
                bt_register.Enabled = false;
                bt_remove.Enabled = false;

                tx_selectedIDSample.Location = new Point(68, 15);
                bt_sampleReplace.Location = new Point(88, 41);
                bt_sampleReplace.Size = new Size(38, 38);
                bt_saveAlterSample.Location = new Point(71, 91);
                bt_sampleReplace.BackgroundImage = Resources.fingerprint_scan;
                bt_saveAlterSample.Enabled = false;
            }
            else
            {
                tc_modify.Visible = false;
                dg_users.Enabled = true;
                dg_users.BackgroundColor = Color.DarkGray;
                dg_users.DefaultCellStyle.SelectionBackColor = Color.DodgerBlue;
                tb_userID.Enabled = true;
                bt_modify.Text = "Modify";

                if (currentDeviceID != -1 && !cb_autoOn.Checked)
                    bt_capture.Enabled = true;
                if (hActivatedFIR != null)
                    bt_register.Enabled = true;
                if (dg_users.SelectedRows.Count > 0)
                    bt_remove.Enabled = true;
            }

        }

        // Save changes to user name
        private void bt_saveAlterUser_Click(object sender, EventArgs e)
        {
            if (char.ToUpper(tb_alterName.Text[0]) + tb_alterName.Text.Substring(1) == user.name)
            {
                MessageBox.Show("Please enter a new name.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (tb_alterName.Text.Any(char.IsDigit) || tb_alterName.Text.Any(c => !char.IsLetter(c) && !char.IsWhiteSpace(c)))
            {
                MessageBox.Show("Please enter a valid name or Cancel the Rename.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataRow row in dg_users.Rows)
            {
                if (row["Name"] + string.Empty == tb_alterName.Text)
                {
                    MessageBox.Show("This name already exists!\nAdd a last name.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (DialogResult.Yes == MessageBox.Show("Do you want to save the changes?", "Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                user.name = tb_alterName.Text;
                sql.UpdateDataUser(user);

                string sample = tb_sample.Text;
                UpdateDGUsers();
                tb_userID.Text = user.id.ToString();
                tb_sample.Text = sample;

                tc_modify.Visible = false;
                dg_users.Enabled = true;
                dg_users.BackgroundColor = Color.DarkGray;
                dg_users.DefaultCellStyle.SelectionBackColor = Color.DodgerBlue;
                tb_userID.Enabled = true;
                bt_modify.Text = "Modify";

                bt_capture.Enabled = true;
                if (hActivatedFIR != null)
                    bt_register.Enabled = true;
                if (dg_users.SelectedRows.Count > 0)
                    bt_remove.Enabled = true;
            }
        }

        // Capture a new FIR to replace the selected sample
        private void bt_sampleReplace_Click(object sender, EventArgs e)
        {
            NBioAPI.Type.HFIR hNewFIR;
            NBioAPI.Type.HFIR hAuditFIR = new NBioAPI.Type.HFIR();

            // Capture FIR
            if (cb_autoOn.Checked)
            {
                NBioAPI.Type.WINDOW_OPTION windowOption = new NBioAPI.Type.WINDOW_OPTION();
                windowOption.WindowStyle = NBioAPI.Type.WINDOW_STYLE.INVISIBLE;
                uint ret = m_NBioAPI.Capture(NBioAPI.Type.FIR_PURPOSE.VERIFY, out hNewFIR, NBioAPI.Type.TIMEOUT.DEFAULT, hAuditFIR, windowOption);
                if (ret != NBioAPI.Error.NONE)
                {
                    ErrorMsg(ret);
                    return;
                }
            }
            else
            {
                m_NBioAPI.OpenDevice(deviceID[currentDeviceID]);
                uint ret = m_NBioAPI.Capture(NBioAPI.Type.FIR_PURPOSE.VERIFY, out hNewFIR, NBioAPI.Type.TIMEOUT.DEFAULT, hAuditFIR, null);
                if (ret != NBioAPI.Error.NONE)
                {
                    ErrorMsg(ret);
                    m_NBioAPI.CloseDevice(deviceID[currentDeviceID]);
                    return;
                }
                m_NBioAPI.CloseDevice(deviceID[currentDeviceID]);
            }

            // Convert the FIR to enroll template
            NBioAPI.Type.HFIR enrollNewFIR = ConvertToEnroll(hNewFIR);

            m_NBioAPI.GetTextFIRFromHandle(enrollNewFIR, out NBioAPI.Type.FIR_TEXTENCODE newTextFIR, true);

            replaseFir.id = user.id;
            replaseFir.hash = newTextFIR.TextFIR;

            NBioAPI.Export.EXPORT_AUDIT_DATA exportAuditData;
            m_Export.NBioBSPToImage(hAuditFIR, out exportAuditData);

            replaseAudit.id = replaseFir.id;
            replaseAudit.data = exportAuditData.AuditData[0].Image[0].Data;
            replaseAudit.imageHeight = exportAuditData.ImageHeight;
            replaseAudit.imageWidth = exportAuditData.ImageWidth;

            tx_selectedIDSample.Location = new Point(13, 33);
            bt_saveAlterSample.Location = new Point(20, 66);
            bt_sampleReplace.Location = new Point(122, 23);
            bt_sampleReplace.Size = new Size(72, 80);

            bt_saveAlterSample.Enabled = true;

            bt_sampleReplace.BackgroundImage = ConvertFIRToJpg(replaseAudit);
        }

        // Save changes to sample
        private void bt_saveAlterSample_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Do you want to save the changes?", "Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                replaseFir.sample = Convert.ToInt32(tb_sample.Text);
                replaseAudit.sample = replaseFir.sample;

                sql.UpdateDataFirAudit(replaseFir, replaseAudit);

                UpdateDGUsers();
                tb_userID.Text = Convert.ToString(replaseFir.id);
                tb_sample.Text = Convert.ToString(replaseFir.sample);

                tc_modify.Visible = false;
                dg_users.Enabled = true;
                dg_users.BackgroundColor = Color.DarkGray;
                dg_users.DefaultCellStyle.SelectionBackColor = Color.DodgerBlue;
                tb_userID.Enabled = true;
                bt_modify.Text = "Modify";

                bt_capture.Enabled = true;
                if (hActivatedFIR != null)
                    bt_register.Enabled = true;
                if (dg_users.SelectedRows.Count > 0)
                    bt_remove.Enabled = true;
            }
        }

        #endregion
    }
}
