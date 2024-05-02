
namespace CRUD_User
{
    partial class CRUD
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (m_IndexSearch != null)
                {
                    m_IndexSearch.ClearDB();
                    m_IndexSearch.TerminateEngine();
                }
                if (m_NBioAPI != null)
                {
                    m_NBioAPI.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CRUD));
            tx_actual = new Label();
            bt_capture = new Button();
            bt_register = new Button();
            tb_userID = new TextBox();
            tx_ID = new Label();
            pb_actvatedFir = new PictureBox();
            dg_users = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)pb_actvatedFir).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dg_users).BeginInit();
            SuspendLayout();
            // 
            // tx_actual
            // 
            resources.ApplyResources(tx_actual, "tx_actual");
            tx_actual.BackColor = Color.Transparent;
            tx_actual.Name = "tx_actual";
            // 
            // bt_capture
            // 
            resources.ApplyResources(bt_capture, "bt_capture");
            bt_capture.Cursor = Cursors.Hand;
            bt_capture.Name = "bt_capture";
            bt_capture.UseVisualStyleBackColor = true;
            bt_capture.Click += bt_capture_Click;
            // 
            // bt_register
            // 
            resources.ApplyResources(bt_register, "bt_register");
            bt_register.Cursor = Cursors.Hand;
            bt_register.Name = "bt_register";
            bt_register.UseVisualStyleBackColor = true;
            bt_register.Click += bt_register_Click;
            // 
            // tb_userID
            // 
            resources.ApplyResources(tb_userID, "tb_userID");
            tb_userID.Name = "tb_userID";
            // 
            // tx_ID
            // 
            resources.ApplyResources(tx_ID, "tx_ID");
            tx_ID.BackColor = Color.Transparent;
            tx_ID.BorderStyle = BorderStyle.FixedSingle;
            tx_ID.Name = "tx_ID";
            // 
            // pb_actvatedFir
            // 
            resources.ApplyResources(pb_actvatedFir, "pb_actvatedFir");
            pb_actvatedFir.BorderStyle = BorderStyle.Fixed3D;
            pb_actvatedFir.Name = "pb_actvatedFir";
            pb_actvatedFir.TabStop = false;
            // 
            // dg_users
            // 
            resources.ApplyResources(dg_users, "dg_users");
            dg_users.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dg_users.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_users.Name = "dg_users";
            dg_users.CellContentClick += dg_users_CellContentClick;
            // 
            // CRUD
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dg_users);
            Controls.Add(tx_actual);
            Controls.Add(pb_actvatedFir);
            Controls.Add(tx_ID);
            Controls.Add(tb_userID);
            Controls.Add(bt_register);
            Controls.Add(bt_capture);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "CRUD";
            ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)pb_actvatedFir).EndInit();
            ((System.ComponentModel.ISupportInitialize)dg_users).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Label tx_actual;
        private Button bt_capture;
        private Button bt_register;
        private TextBox tb_userID;
        private Label tx_ID;
        private PictureBox pb_actvatedFir;
        private DataGridView dg_users;
    }
}
