
namespace CRUD_fingertech
{
    partial class FingerTech
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
            tx_actual = new Label();
            bt_capture = new Button();
            tb_ActivatedCapture = new TextBox();
            bt_register = new Button();
            tb_userID = new TextBox();
            tx_ID = new Label();
            dg_users = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dg_users).BeginInit();
            SuspendLayout();
            // 
            // tx_actual
            // 
            tx_actual.AutoSize = true;
            tx_actual.BackColor = Color.Transparent;
            tx_actual.Font = new Font("Nirmala UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tx_actual.Location = new Point(32, 18);
            tx_actual.Name = "tx_actual";
            tx_actual.Size = new Size(82, 17);
            tx_actual.TabIndex = 0;
            tx_actual.Text = "ActivatedFir";
            // 
            // bt_capture
            // 
            bt_capture.Location = new Point(32, 184);
            bt_capture.Margin = new Padding(23, 3, 3, 3);
            bt_capture.Name = "bt_capture";
            bt_capture.Size = new Size(75, 23);
            bt_capture.TabIndex = 1;
            bt_capture.Text = "Capture";
            bt_capture.UseVisualStyleBackColor = true;
            bt_capture.Click += bt_capture_Click;
            // 
            // tb_ActivatedCapture
            // 
            tb_ActivatedCapture.BackColor = SystemColors.HighlightText;
            tb_ActivatedCapture.Location = new Point(21, 27);
            tb_ActivatedCapture.Multiline = true;
            tb_ActivatedCapture.Name = "tb_ActivatedCapture";
            tb_ActivatedCapture.ReadOnly = true;
            tb_ActivatedCapture.Size = new Size(352, 151);
            tb_ActivatedCapture.TabIndex = 4;
            // 
            // bt_register
            // 
            bt_register.Location = new Point(278, 184);
            bt_register.Name = "bt_register";
            bt_register.Size = new Size(75, 23);
            bt_register.TabIndex = 5;
            bt_register.Text = "Register";
            bt_register.UseVisualStyleBackColor = true;
            bt_register.Click += bt_register_Click;
            // 
            // tb_userID
            // 
            tb_userID.Location = new Point(190, 184);
            tb_userID.Name = "tb_userID";
            tb_userID.Size = new Size(82, 23);
            tb_userID.TabIndex = 6;
            tb_userID.TextAlign = HorizontalAlignment.Right;
            // 
            // tx_ID
            // 
            tx_ID.AutoSize = true;
            tx_ID.BackColor = Color.Transparent;
            tx_ID.BorderStyle = BorderStyle.FixedSingle;
            tx_ID.Font = new Font("Nirmala UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tx_ID.Location = new Point(190, 184);
            tx_ID.Name = "tx_ID";
            tx_ID.Size = new Size(55, 22);
            tx_ID.TabIndex = 7;
            tx_ID.Text = "UserID";
            // 
            // dg_users
            // 
            dg_users.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_users.Location = new Point(475, 18);
            dg_users.Name = "dg_users";
            dg_users.Size = new Size(313, 407);
            dg_users.TabIndex = 11;
            // 
            // FingerTech
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dg_users);
            Controls.Add(tx_ID);
            Controls.Add(tb_userID);
            Controls.Add(bt_register);
            Controls.Add(tx_actual);
            Controls.Add(tb_ActivatedCapture);
            Controls.Add(bt_capture);
            Name = "FingerTech";
            Text = "FingerTech";
            ((System.ComponentModel.ISupportInitialize)dg_users).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Label tx_actual;
        private Button bt_capture;
        private TextBox tb_ActivatedCapture;
        private Button bt_register;
        private TextBox tb_userID;
        private Label tx_ID;
        private DataGridView dg_users;
    }
}
