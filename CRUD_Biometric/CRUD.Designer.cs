namespace CRUD_Biometric
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

        private void InitializeComponent()
        {
            pb_actvatedFir = new PictureBox();
            tx_actual = new Label();
            bt_capture = new Button();
            tx_ID = new Label();
            tb_userID = new TextBox();
            bt_register = new Button();
            dg_users = new DataGridView();
            tx_selected = new Label();
            pb_selectedFir = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pb_actvatedFir).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dg_users).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_selectedFir).BeginInit();
            SuspendLayout();
            // 
            // pb_actvatedFir
            // 
            pb_actvatedFir.BorderStyle = BorderStyle.Fixed3D;
            pb_actvatedFir.Location = new Point(21, 27);
            pb_actvatedFir.Name = "pb_actvatedFir";
            pb_actvatedFir.Size = new Size(124, 146);
            pb_actvatedFir.SizeMode = PictureBoxSizeMode.Zoom;
            pb_actvatedFir.TabIndex = 0;
            pb_actvatedFir.TabStop = false;
            // 
            // tx_actual
            // 
            tx_actual.AutoSize = true;
            tx_actual.Font = new Font("Nirmala UI", 9.75F, FontStyle.Bold);
            tx_actual.Location = new Point(32, 18);
            tx_actual.Name = "tx_actual";
            tx_actual.Size = new Size(82, 17);
            tx_actual.TabIndex = 1;
            tx_actual.Text = "ActivatedFir";
            // 
            // bt_capture
            // 
            bt_capture.Cursor = Cursors.Hand;
            bt_capture.Font = new Font("Nirmala UI", 9F);
            bt_capture.Location = new Point(48, 179);
            bt_capture.Name = "bt_capture";
            bt_capture.Size = new Size(75, 23);
            bt_capture.TabIndex = 2;
            bt_capture.Text = "Capture";
            bt_capture.UseVisualStyleBackColor = true;
            bt_capture.Click += bt_capture_Click;
            // 
            // tx_ID
            // 
            tx_ID.AutoSize = true;
            tx_ID.BackColor = Color.Transparent;
            tx_ID.BorderStyle = BorderStyle.FixedSingle;
            tx_ID.Font = new Font("Nirmala UI", 11.25F);
            tx_ID.Location = new Point(194, 184);
            tx_ID.Name = "tx_ID";
            tx_ID.Size = new Size(55, 22);
            tx_ID.TabIndex = 3;
            tx_ID.Text = "UserID";
            // 
            // tb_userID
            // 
            tb_userID.Location = new Point(194, 184);
            tb_userID.Name = "tb_userID";
            tb_userID.Size = new Size(82, 23);
            tb_userID.TabIndex = 4;
            tb_userID.TextAlign = HorizontalAlignment.Right;
            // 
            // bt_register
            // 
            bt_register.Cursor = Cursors.Hand;
            bt_register.Font = new Font("Nirmala UI", 9F);
            bt_register.Location = new Point(284, 184);
            bt_register.Name = "bt_register";
            bt_register.Size = new Size(75, 23);
            bt_register.TabIndex = 5;
            bt_register.Text = "Register";
            bt_register.UseVisualStyleBackColor = true;
            bt_register.Click += bt_register_Click;
            // 
            // dg_users
            // 
            dg_users.AllowUserToAddRows = false;
            dg_users.AllowUserToDeleteRows = false;
            dg_users.AllowUserToOrderColumns = true;
            dg_users.AllowUserToResizeColumns = false;
            dg_users.AllowUserToResizeRows = false;
            dg_users.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dg_users.CausesValidation = false;
            dg_users.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_users.EditMode = DataGridViewEditMode.EditProgrammatically;
            dg_users.Location = new Point(384, 18);
            dg_users.MultiSelect = false;
            dg_users.Name = "dg_users";
            dg_users.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dg_users.ShowCellToolTips = false;
            dg_users.ShowEditingIcon = false;
            dg_users.Size = new Size(268, 388);
            dg_users.TabIndex = 6;
            dg_users.SelectionChanged += dg_users_SelectionChanged;
            // 
            // tx_selected
            // 
            tx_selected.AutoSize = true;
            tx_selected.Font = new Font("Nirmala UI", 9.75F, FontStyle.Bold);
            tx_selected.Location = new Point(246, 18);
            tx_selected.Name = "tx_selected";
            tx_selected.Size = new Size(75, 17);
            tx_selected.TabIndex = 8;
            tx_selected.Text = "SelectedFir";
            // 
            // pb_selectedFir
            // 
            pb_selectedFir.BorderStyle = BorderStyle.Fixed3D;
            pb_selectedFir.Location = new Point(235, 27);
            pb_selectedFir.Name = "pb_selectedFir";
            pb_selectedFir.Size = new Size(124, 146);
            pb_selectedFir.SizeMode = PictureBoxSizeMode.Zoom;
            pb_selectedFir.TabIndex = 7;
            pb_selectedFir.TabStop = false;
            // 
            // CRUD
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(665, 418);
            Controls.Add(tx_selected);
            Controls.Add(pb_selectedFir);
            Controls.Add(dg_users);
            Controls.Add(bt_register);
            Controls.Add(tx_ID);
            Controls.Add(bt_capture);
            Controls.Add(tx_actual);
            Controls.Add(pb_actvatedFir);
            Controls.Add(tb_userID);
            Name = "CRUD";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CRUD";
            ((System.ComponentModel.ISupportInitialize)pb_actvatedFir).EndInit();
            ((System.ComponentModel.ISupportInitialize)dg_users).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_selectedFir).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dg_users;
        private PictureBox pb_actvatedFir;
        private TextBox tb_userID;
        private Button bt_capture;
        private Button bt_register;
        private Label tx_actual;
        private Label tx_ID;
        private Label tx_selected;
        private PictureBox pb_selectedFir;
    }
}
