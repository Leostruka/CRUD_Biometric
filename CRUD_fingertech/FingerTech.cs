using CRUD_User.Model;
using CRUD_User.View;
using NITGEN.SDK.NBioBSP;


namespace CRUD_fingertech
{
    public partial class FingerTech : Form
    {
        NBioAPI m_NBioAPI;
        NBioAPI.IndexSearch m_IndexSearch;

        NBioAPI.Type.HFIR hActivatedFIR;
        UserModel.User user;

        public FingerTech()
        {
            InitializeComponent();

            // Initialize NBioAPI
            m_NBioAPI = new NBioAPI();
            m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            user = new UserModel.User();

            // Start IndexSearch
            uint ret = m_IndexSearch.InitEngine();
            if (ret != NBioAPI.Error.NONE)
            {
                ErrorMsg(ret);
                return;
            }

            // Set default first user ID
            tb_userID.Text = "1";

            // Clear displays
            tb_ActivatedCapture.Text = string.Empty;
            lv_users.Items.Clear();
        }

        // Error message
        private void ErrorMsg(uint ret)
        {
            MessageBox.Show("Error: " + ret.ToString() + "\n" + NBioAPI.Error.GetErrorDescription(ret), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Update ActivateCapture
        private void AttActivateCapture(NBioAPI.Type.HFIR hFIR)
        {
            // Get text FIR
            NBioAPI.Type.FIR_TEXTENCODE textFIR;
            m_NBioAPI.GetTextFIRFromHandle(hFIR, out textFIR, true);

            // Add item to tb_ActivatedCapture
            tb_ActivatedCapture.Text = textFIR.TextFIR;
            tx_actual.Location = new Point(32, 10);
            hActivatedFIR = hFIR;

        }

        private void bt_capture_Click(object sender, EventArgs e)
        {
            NBioAPI.Type.HFIR hNewFIR;

            // Clear tb_ActivatedCapture
            tb_ActivatedCapture.Text = string.Empty;

            // Capture FIR
            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            uint ret = m_NBioAPI.Capture(out hNewFIR);
            if (ret != NBioAPI.Error.NONE)
            {
                ErrorMsg(ret);
                m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
                return;
            }

            m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            
            // Activate FIR
            AttActivateCapture(hNewFIR);
        }

        private void bt_register_Click(object sender, EventArgs e)
        {
            if (hActivatedFIR == null)
            {
                MessageBox.Show("Please, capture a FIR first!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NBioAPI.Type.HFIR hCapturedFIR;
            uint userID = 0;

            // Verify if ID is valid
            try
            {
                int test = Convert.ToInt32(tb_userID.Text, 10);
                if (test == 0)
                {
                    throw(new Exception());
                }
            }
            catch
            {
                MessageBox.Show("Invalid ID!\n User ID must be have numeric type and greated than 0.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get ID
            userID = Convert.ToUInt32(tb_userID.Text, 10);

            // Capture FIR2
            MessageBox.Show("Please, put the same finger of the capture!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            uint ret = m_NBioAPI.Capture(out hCapturedFIR);
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
                // Set UserID
                user.id = (int)userID;

                // Open UserInformationView
                UserInformationView userInformationView = new UserInformationView();
                userInformationView.ShowDialog();

                // Get user information
                user.name = userInformationView.GetName();
                MessageBox.Show("Name:" + user);

                // Register in to database


                // Register FIR to IndexSearchDB

                MessageBox.Show("User ID: " + userID.ToString() + " registered!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("FIRs do not match!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
