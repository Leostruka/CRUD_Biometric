using CRUD_Biometric.Model;
using CRUD_User.View;
using CRUD_Biometric.DataAccess;
using NITGEN.SDK.NBioBSP;
using System.Data;
using CRUD_Biometric.Model;

namespace CRUD_Biometric
{
    public partial class CRUD : Form
    {
        NBioAPI m_NBioAPI;
        NBioAPI.Export m_Export;
        NBioAPI.IndexSearch m_IndexSearch;

        NBioAPI.Type.HFIR hActivatedFIR;
        UserModel.User user;
        FIRModel.FIR fir;
        AuditModel.Audit audit;

        DataTable dt_user_fir;

        SQL sql;

        public CRUD()
        {
            InitializeComponent();

            // Initialize NBioAPI
            m_NBioAPI = new NBioAPI();
            m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            m_Export = new NBioAPI.Export(m_NBioAPI);
            uint ret = m_IndexSearch.InitEngine();
            if (ret != NBioAPI.Error.NONE)
            {
                ErrorMsg(ret);
                this.Close();
            }
            NBioAPI.Type.VERSION version = new NBioAPI.Type.VERSION();
            m_NBioAPI.GetVersion(out version);

            user = new UserModel.User();
            fir = new FIRModel.FIR();
            audit = new AuditModel.Audit();
            // Update dg_users
            UpdateDGUsers();
        }

        // Error message
        private void ErrorMsg(uint ret) => MessageBox.Show("Error: " + ret.ToString() + "\n" + NBioAPI.Error.GetErrorDescription(ret), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        // Update IndexSearchDB
        private void UpdateIndexSearch()
        {
            m_IndexSearch.ClearDB();
            // Set SQL
            sql = new SQL();
            DataTable dt_fir = sql.GetDataFir();

            // Add FIR to IndexSearchDB if exists in database
            if (dt_fir.Rows.Count > 0)
            {
                NBioAPI.IndexSearch.FP_INFO[] fpinfo;
                for (int i = 0; i < dt_fir.Rows.Count; i++)
                {
                    NBioAPI.Type.FIR_TEXTENCODE textFIR = new NBioAPI.Type.FIR_TEXTENCODE();

                    textFIR.TextFIR = dt_fir.Rows[i]["hash"].ToString();
                    string id = dt_fir.Rows[i]["id"].ToString() + "909" + dt_fir.Rows[i]["sample"].ToString();
                    uint ret = m_IndexSearch.AddFIR(textFIR, Convert.ToUInt32(id), out fpinfo);
                    if (ret != NBioAPI.Error.NONE)
                    {
                        ErrorMsg(ret);
                        this.Close();
                    }
                }
            }
        }

        // Update dg_users based on database
        private void UpdateDGUsers()
        {
            // Set SQL and get data
            sql = new SQL();
            dt_user_fir = sql.GetDataUserFir();

            // Clear existing data in dg_users
            dg_users.Rows.Clear();
            dg_users.Columns.Clear();

            // Add columns to dg_users
            dg_users.Columns.Add("ID", "ID");
            dg_users.Columns.Add("Name", "Name");
            dg_users.Columns.Add("Samples", "Sample amount");

            // Populate dg_users with user information
            var userSamples = new Dictionary<int, int>(); // Dictionary to store the number of samples for each user
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
            UpdateIndexSearch();
        }

        private Image ConvertFIRToJpg(NBioAPI.Type.HFIR hFIR)
        {
            NBioAPI.Export.EXPORT_AUDIT_DATA exportAuditData;
            m_Export.NBioBSPToImage(hFIR, out exportAuditData);

            audit.data = exportAuditData.AuditData[0].Image[0].Data;
            audit.imageHeight = exportAuditData.ImageHeight;
            audit.imageWidth = exportAuditData.ImageWidth;

            uint r = m_NBioAPI.ImgConvRawToJpgBuf(audit.data, audit.imageWidth, audit.imageHeight, 100, out byte[] outbuffer);
            if (r != NBioAPI.Error.NONE)
            {
                ErrorMsg(r);
                return null;
            }

            return Image.FromStream(new MemoryStream(outbuffer));
        }

        private Image ConvertFIRToJpg(AuditModel.Audit audit)
        {
            uint r = m_NBioAPI.ImgConvRawToJpgBuf(audit.data, audit.imageWidth, audit.imageHeight, 100, out byte[] outbuffer);
            if (r != NBioAPI.Error.NONE)
            {
                ErrorMsg(r);
                return null;
            }

            return Image.FromStream(new MemoryStream(outbuffer));
        }

        private void AttSelectFir(int id, int sample)
        {
            AuditModel.Audit audit = new AuditModel.Audit();
            audit.id = id;

            for (int i = 0; i < dt_user_fir.Rows.Count; i++)
            {
                if (Convert.ToInt32(dt_user_fir.Rows[i]["id"]) == audit.id)
                {
                    DataTable dt_audit = sql.GetSpecificDataUserFirAudit(audit.id);

                    foreach (DataRow row in dt_audit.Rows)
                    {
                        if (Convert.ToInt32(row["sample"]) == sample)
                        {
                            audit.data = StringToByteArray(row["data"].ToString());
                            audit.imageWidth = Convert.ToUInt32(row["imageWidth"]);
                            audit.imageHeight = Convert.ToUInt32(row["imageHeight"]);
                        }
                    }

                    pb_selectedFir.Image = ConvertFIRToJpg(audit);
                    pb_selectedFir.Size = new Size(124, 146);
                    tx_selected.Location = new Point(235, 10);

                    tb_userID.Text = audit.id.ToString();

                    break;
                }
            }
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        // Update ActivateCapture
        private void AttActivateCapture(NBioAPI.Type.HFIR hFIR, NBioAPI.Type.HFIR hAuditFIR)
        {
            // Add item to tb_ActivatedCapture
            pb_actvatedFir.Image = ConvertFIRToJpg(hAuditFIR);
            pb_actvatedFir.Size = new Size(124, 146);
            tx_actual.Location = new Point(32, 10);
            hActivatedFIR = hFIR;

            bt_register.Enabled = true;
        }

        private void bt_capture_Click(object sender, EventArgs e)
        {
            NBioAPI.Type.HFIR hNewFIR;

            // Clear pb_actvaredFir
            pb_actvatedFir.Image = null;

            // Capture FIR
            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);

            NBioAPI.Type.HFIR hAuditFIR = new NBioAPI.Type.HFIR();

            uint ret = m_NBioAPI.Capture(NBioAPI.Type.FIR_PURPOSE.VERIFY, out hNewFIR, NBioAPI.Type.TIMEOUT.DEFAULT, hAuditFIR, null);
            if (ret != NBioAPI.Error.NONE)
            {
                ErrorMsg(ret);
                m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
                return;
            }

            m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);

            // Activate FIR
            AttActivateCapture(hNewFIR, hAuditFIR);
        }

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

            string id = fpInfo.ID.ToString();
            int index = id.IndexOf("909");
            string before909 = id.Substring(0, index);
            string after909 = id.Substring(index + 3);
            fpInfo.ID = Convert.ToUInt32(before909);
            fpInfo.SampleNumber = (byte)Convert.ToUInt32(after909);

            if (fpInfo.ID != 0)
            {
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
                    m_NBioAPI.GetTextFIRFromHandle(hActivatedFIR, out NBioAPI.Type.FIR_TEXTENCODE newTextFIR, true);
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
                    bt_register.Enabled = false;

                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                foreach (DataRow row in dt_user_fir.Rows)
                {
                    if (Convert.ToInt32(row["id"]) == userID)
                    {
                        if (DialogResult.Yes == MessageBox.Show("The user ID: " + (int)userID + " already exists!\nName: " + row["name"] + "\nRegistry anyway?", "Existing User!", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            m_NBioAPI.GetTextFIRFromHandle(hActivatedFIR, out NBioAPI.Type.FIR_TEXTENCODE newTextFIR, true);
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


            // Capture FIR2
            MessageBox.Show("Please, put the same finger of the capture!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            uint ret = m_NBioAPI.Capture(NBioAPI.Type.FIR_PURPOSE.VERIFY, out hCapturedFIR, NBioAPI.Type.TIMEOUT.DEFAULT, hAuditFIR, null);
            if (ret != NBioAPI.Error.NONE)
            {
                ErrorMsg(ret);
                m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
                return;
            }
            m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);

            // Get FIR2 and compare with activated FIR
            m_NBioAPI.GetTextFIRFromHandle(hActivatedFIR, out NBioAPI.Type.FIR_TEXTENCODE textFIR, true);
            m_NBioAPI.GetTextFIRFromHandle(hCapturedFIR, out NBioAPI.Type.FIR_TEXTENCODE textFIR2, true);
            m_NBioAPI.VerifyMatch(textFIR, textFIR2, out bool result, null);

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

                MessageBox.Show("User ID: " + userID.ToString() + "\nName: " + user.name + "\nregistered!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tb_userID.Text = (userID + 1).ToString();

                bt_register.Enabled = false;
            }
            else
            {
                MessageBox.Show("FIRs do not match!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Update dg_users
            UpdateDGUsers();
        }

        private void bt_modify_Click(object sender, EventArgs e)
        {

        }

        private void bt_remove_Click(object sender, EventArgs e)
        {
            sql.DeleteDataFirAudit(Convert.ToInt32(tb_userID.Text), Convert.ToInt32(tb_sample.Text));
            UpdateDGUsers();
            bt_returnSample_Click(sender, e);
        }

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
                        break;
                    }
                }
            }
        }

        private void tb_userID_TextChanged(object sender, EventArgs e)
        {
            // Check if the user ID is valid
            if (string.IsNullOrEmpty(tb_userID.Text) || !int.TryParse(tb_userID.Text, out _))
            {
                tb_userID.Text = "1";
            }
            else if (int.Parse(tb_userID.Text) < 1)
            {
                tb_userID.Text = "1";
            }
            AttSelectFir(int.Parse(tb_userID.Text), 1);
        }

        private void tb_sample_TextChanged(object sender, EventArgs e)
        {
            tb_sample.Focus();
            AttSelectFir(int.Parse(tb_userID.Text), int.Parse(tb_sample.Text));
        }

        private void bt_returnSample_Click(object sender, EventArgs e)
        {
            int currentSample = int.Parse(tb_sample.Text) - 1;

            for (int i = dt_user_fir.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dt_user_fir.Rows[i];
                if (Convert.ToInt32(row["id"]) == Convert.ToInt32(tb_userID.Text) && Convert.ToInt32(row["sample"]) <= currentSample)
                {
                    currentSample = Convert.ToInt32(row["sample"]);
                    break;
                }
            }
            tb_sample.Text = currentSample.ToString();

            if (currentSample == 1)
            {
                bt_returnSample.Enabled = false;
                bt_nextSample.Enabled = true;
            }
            else
            {
                bt_nextSample.Enabled = true;
            }
        }

        private void bt_nextSample_Click(object sender, EventArgs e)
        {
            int currentSample = int.Parse(tb_sample.Text) + 1;
            int lastSample = 0;

            foreach (DataRow row in dt_user_fir.Rows)
            {
                if (Convert.ToInt32(row["id"]) == Convert.ToInt32(tb_userID.Text) && Convert.ToInt32(row["sample"]) >= currentSample)
                {
                    currentSample = Convert.ToInt32(row["sample"]);
                    lastSample = dt_user_fir.AsEnumerable().Last(row => Convert.ToInt32(row["id"]) == Convert.ToInt32(tb_userID.Text)).Field<int>("sample");
                    break;
                }
            }
            tb_sample.Text = currentSample.ToString();

            if (currentSample == lastSample)
            {
                bt_nextSample.Enabled = false;
                bt_returnSample.Enabled = true;
            }
            else
            {
                bt_returnSample.Enabled = true;
            }
        }
    }
}
