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
            bt_remove = new Button();
            tx_sample = new Label();
            tb_sample = new TextBox();
            bt_returnSample = new Button();
            bt_nextSample = new Button();
            bt_modify = new Button();
            tx_sampleCount = new Label();
            tc_modify = new TabControl();
            tp_user = new TabPage();
            lb = new Label();
            tp_sample = new TabPage();
            ((System.ComponentModel.ISupportInitialize)pb_actvatedFir).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dg_users).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pb_selectedFir).BeginInit();
            tc_modify.SuspendLayout();
            tp_user.SuspendLayout();
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
            tb_userID.MaxLength = 4;
            tb_userID.Name = "tb_userID";
            tb_userID.Size = new Size(82, 23);
            tb_userID.TabIndex = 4;
            tb_userID.TextAlign = HorizontalAlignment.Right;
            tb_userID.TextChanged += tb_userID_TextChanged;
            // 
            // bt_register
            // 
            bt_register.Cursor = Cursors.Hand;
            bt_register.Enabled = false;
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
            dg_users.AllowUserToResizeColumns = false;
            dg_users.AllowUserToResizeRows = false;
            dg_users.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dg_users.CausesValidation = false;
            dg_users.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dg_users.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dg_users.EditMode = DataGridViewEditMode.EditProgrammatically;
            dg_users.Location = new Point(384, 18);
            dg_users.MultiSelect = false;
            dg_users.Name = "dg_users";
            dg_users.RowHeadersVisible = false;
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
            // bt_remove
            // 
            bt_remove.Cursor = Cursors.Hand;
            bt_remove.Enabled = false;
            bt_remove.Font = new Font("Nirmala UI", 9F);
            bt_remove.Location = new Point(284, 213);
            bt_remove.Name = "bt_remove";
            bt_remove.Size = new Size(75, 23);
            bt_remove.TabIndex = 9;
            bt_remove.Text = "Remove";
            bt_remove.UseVisualStyleBackColor = true;
            bt_remove.Click += bt_remove_Click;
            // 
            // tx_sample
            // 
            tx_sample.AutoSize = true;
            tx_sample.BackColor = Color.Transparent;
            tx_sample.BorderStyle = BorderStyle.FixedSingle;
            tx_sample.Font = new Font("Nirmala UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tx_sample.Location = new Point(194, 213);
            tx_sample.Name = "tx_sample";
            tx_sample.Size = new Size(57, 15);
            tx_sample.TabIndex = 10;
            tx_sample.Text = "SampleID";
            // 
            // tb_sample
            // 
            tb_sample.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_sample.Location = new Point(235, 213);
            tb_sample.MaxLength = 3;
            tb_sample.Name = "tb_sample";
            tb_sample.Size = new Size(41, 22);
            tb_sample.TabIndex = 11;
            tb_sample.TextAlign = HorizontalAlignment.Right;
            tb_sample.TextChanged += tb_sample_TextChanged;
            // 
            // bt_returnSample
            // 
            bt_returnSample.Enabled = false;
            bt_returnSample.FlatStyle = FlatStyle.Popup;
            bt_returnSample.Font = new Font("Segoe UI Symbol", 8.25F, FontStyle.Bold);
            bt_returnSample.Location = new Point(194, 228);
            bt_returnSample.Name = "bt_returnSample";
            bt_returnSample.Size = new Size(24, 22);
            bt_returnSample.TabIndex = 12;
            bt_returnSample.Text = "<";
            bt_returnSample.TextAlign = ContentAlignment.TopCenter;
            bt_returnSample.UseVisualStyleBackColor = true;
            bt_returnSample.Click += bt_returnSample_Click;
            // 
            // bt_nextSample
            // 
            bt_nextSample.FlatStyle = FlatStyle.Popup;
            bt_nextSample.Font = new Font("Segoe UI Symbol", 8.25F, FontStyle.Bold);
            bt_nextSample.Location = new Point(216, 228);
            bt_nextSample.Name = "bt_nextSample";
            bt_nextSample.Size = new Size(24, 22);
            bt_nextSample.TabIndex = 13;
            bt_nextSample.Text = ">";
            bt_nextSample.TextAlign = ContentAlignment.TopCenter;
            bt_nextSample.UseVisualStyleBackColor = true;
            bt_nextSample.Click += bt_nextSample_Click;
            // 
            // bt_modify
            // 
            bt_modify.Cursor = Cursors.Hand;
            bt_modify.Enabled = false;
            bt_modify.Font = new Font("Nirmala UI", 9F);
            bt_modify.Location = new Point(246, 251);
            bt_modify.Name = "bt_modify";
            bt_modify.Size = new Size(113, 23);
            bt_modify.TabIndex = 14;
            bt_modify.Text = "Modify";
            bt_modify.UseVisualStyleBackColor = true;
            bt_modify.Click += bt_modify_Click;
            // 
            // tx_sampleCount
            // 
            tx_sampleCount.AutoSize = true;
            tx_sampleCount.Location = new Point(240, 234);
            tx_sampleCount.Name = "tx_sampleCount";
            tx_sampleCount.Size = new Size(0, 18);
            tx_sampleCount.TabIndex = 15;
            tx_sampleCount.UseCompatibleTextRendering = true;
            // 
            // tc_modify
            // 
            tc_modify.Appearance = TabAppearance.Buttons;
            tc_modify.Controls.Add(tp_user);
            tc_modify.Controls.Add(tp_sample);
            tc_modify.Font = new Font("Nirmala UI", 9F);
            tc_modify.Location = new Point(12, 256);
            tc_modify.Name = "tc_modify";
            tc_modify.SelectedIndex = 0;
            tc_modify.Size = new Size(347, 150);
            tc_modify.TabIndex = 16;
            tc_modify.Tag = "";
            tc_modify.Visible = false;
            // 
            // tp_user
            // 
            tp_user.Controls.Add(lb);
            tp_user.Font = new Font("Nirmala UI", 9F);
            tp_user.Location = new Point(4, 27);
            tp_user.Name = "tp_user";
            tp_user.Padding = new Padding(3);
            tp_user.Size = new Size(339, 119);
            tp_user.TabIndex = 0;
            tp_user.Text = "User";
            tp_user.UseVisualStyleBackColor = true;
            // 
            // lb
            // 
            lb.AutoSize = true;
            lb.Location = new Point(91, 12);
            lb.Name = "lb";
            lb.Size = new Size(38, 15);
            lb.TabIndex = 0;
            lb.Text = "label1";
            // 
            // tp_sample
            // 
            tp_sample.Location = new Point(4, 27);
            tp_sample.Name = "tp_sample";
            tp_sample.Padding = new Padding(3);
            tp_sample.Size = new Size(339, 119);
            tp_sample.TabIndex = 1;
            tp_sample.Text = "Sample";
            tp_sample.UseVisualStyleBackColor = true;
            // 
            // CRUD
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(665, 418);
            Controls.Add(bt_modify);
            Controls.Add(bt_nextSample);
            Controls.Add(bt_returnSample);
            Controls.Add(tx_sample);
            Controls.Add(bt_remove);
            Controls.Add(tx_selected);
            Controls.Add(pb_selectedFir);
            Controls.Add(dg_users);
            Controls.Add(bt_register);
            Controls.Add(tx_ID);
            Controls.Add(bt_capture);
            Controls.Add(tx_actual);
            Controls.Add(pb_actvatedFir);
            Controls.Add(tb_userID);
            Controls.Add(tb_sample);
            Controls.Add(tx_sampleCount);
            Controls.Add(tc_modify);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "CRUD";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CRUD";
            ((System.ComponentModel.ISupportInitialize)pb_actvatedFir).EndInit();
            ((System.ComponentModel.ISupportInitialize)dg_users).EndInit();
            ((System.ComponentModel.ISupportInitialize)pb_selectedFir).EndInit();
            tc_modify.ResumeLayout(false);
            tp_user.ResumeLayout(false);
            tp_user.PerformLayout();
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
        private Button bt_remove;
        private Label tx_sample;
        private TextBox tb_sample;
        private Button bt_returnSample;
        private Button bt_nextSample;
        private Button bt_modify;
        private Label tx_sampleCount;
        private TabControl tc_modify;
        private TabPage tp_user;
        private TabPage tp_sample;
        private Label lb;
    }
}
