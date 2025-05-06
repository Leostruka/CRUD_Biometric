using System.ComponentModel;

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
                if (cb_autoOn.Checked)
                {
                    m_NBioAPI.CloseDevice(deviceID[currentDeviceID]);
                }
                if (m_NBioAPI != null)
                {
                    m_NBioAPI.Dispose();
                }

                // Stop the WMI event watcher
                arrivalWatcher.Stop();
                arrivalWatcher.Dispose();

                removalWatcher.Stop();
                removalWatcher.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
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
            tx_selectedID = new Label();
            bt_saveAlterUser = new Button();
            tb_alterName = new TextBox();
            tp_sample = new TabPage();
            bt_sampleReplace = new Button();
            tx_selectedIDSample = new Label();
            bt_saveAlterSample = new Button();
            flp_devices = new FlowLayoutPanel();
            tx_deviceName = new Label();
            pn_deviceInf = new Panel();
            cb_autoOn = new CheckBox();
            tx_autoOn = new Label();
            tb_serialN = new TextBox();
            tx_serialNumber = new Label();
            tb_deviceName = new TextBox();
            fingerCheckWorker = new BackgroundWorker();
            tx_NBioV = new Label();
            ((ISupportInitialize)pb_actvatedFir).BeginInit();
            ((ISupportInitialize)dg_users).BeginInit();
            ((ISupportInitialize)pb_selectedFir).BeginInit();
            tc_modify.SuspendLayout();
            tp_user.SuspendLayout();
            tp_sample.SuspendLayout();
            pn_deviceInf.SuspendLayout();
            SuspendLayout();
            // 
            // pb_actvatedFir
            // 
            pb_actvatedFir.BorderStyle = BorderStyle.Fixed3D;
            pb_actvatedFir.Location = new Point(24, 29);
            pb_actvatedFir.Name = "pb_actvatedFir";
            pb_actvatedFir.Size = new Size(141, 155);
            pb_actvatedFir.SizeMode = PictureBoxSizeMode.Zoom;
            pb_actvatedFir.TabIndex = 0;
            pb_actvatedFir.TabStop = false;
            // 
            // tx_actual
            // 
            tx_actual.AutoSize = true;
            tx_actual.Font = new Font("Montserrat", 8.999999F);
            tx_actual.Location = new Point(37, 19);
            tx_actual.Name = "tx_actual";
            tx_actual.Size = new Size(82, 18);
            tx_actual.TabIndex = 1;
            tx_actual.Text = "ActivatedFir";
            // 
            // bt_capture
            // 
            bt_capture.Cursor = Cursors.Hand;
            bt_capture.Font = new Font("Montserrat", 8.999999F);
            bt_capture.Location = new Point(55, 191);
            bt_capture.Name = "bt_capture";
            bt_capture.Size = new Size(86, 25);
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
            tx_ID.Font = new Font("Montserrat", 8.999999F);
            tx_ID.Location = new Point(222, 196);
            tx_ID.Name = "tx_ID";
            tx_ID.Size = new Size(51, 20);
            tx_ID.TabIndex = 3;
            tx_ID.Text = "UserID";
            // 
            // tb_userID
            // 
            tb_userID.Font = new Font("Montserrat", 8.999999F);
            tb_userID.Location = new Point(222, 196);
            tb_userID.MaxLength = 4;
            tb_userID.Name = "tb_userID";
            tb_userID.Size = new Size(93, 22);
            tb_userID.TabIndex = 4;
            tb_userID.TextAlign = HorizontalAlignment.Right;
            tb_userID.TextChanged += tb_userID_TextChanged;
            // 
            // bt_register
            // 
            bt_register.Cursor = Cursors.Hand;
            bt_register.Enabled = false;
            bt_register.Font = new Font("Montserrat", 8.999999F);
            bt_register.Location = new Point(325, 196);
            bt_register.Name = "bt_register";
            bt_register.Size = new Size(86, 25);
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
            dg_users.BackgroundColor = Color.DarkGray;
            dg_users.CausesValidation = false;
            dg_users.CellBorderStyle = DataGridViewCellBorderStyle.SunkenHorizontal;
            dg_users.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Montserrat", 8.999999F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dg_users.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dg_users.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Montserrat", 8.999999F);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dg_users.DefaultCellStyle = dataGridViewCellStyle2;
            dg_users.EditMode = DataGridViewEditMode.EditProgrammatically;
            dg_users.Location = new Point(439, 19);
            dg_users.MultiSelect = false;
            dg_users.Name = "dg_users";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Montserrat", 8.999999F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dg_users.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dg_users.RowHeadersVisible = false;
            dg_users.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dg_users.ShowCellToolTips = false;
            dg_users.ShowEditingIcon = false;
            dg_users.Size = new Size(306, 414);
            dg_users.TabIndex = 6;
            dg_users.SelectionChanged += dg_users_SelectionChanged;
            // 
            // tx_selected
            // 
            tx_selected.AutoSize = true;
            tx_selected.Font = new Font("Montserrat", 8.999999F);
            tx_selected.Location = new Point(281, 19);
            tx_selected.Name = "tx_selected";
            tx_selected.Size = new Size(75, 18);
            tx_selected.TabIndex = 8;
            tx_selected.Text = "SelectedFir";
            // 
            // pb_selectedFir
            // 
            pb_selectedFir.BorderStyle = BorderStyle.Fixed3D;
            pb_selectedFir.Location = new Point(269, 29);
            pb_selectedFir.Name = "pb_selectedFir";
            pb_selectedFir.Size = new Size(141, 155);
            pb_selectedFir.SizeMode = PictureBoxSizeMode.Zoom;
            pb_selectedFir.TabIndex = 7;
            pb_selectedFir.TabStop = false;
            // 
            // bt_remove
            // 
            bt_remove.Cursor = Cursors.Hand;
            bt_remove.Enabled = false;
            bt_remove.Font = new Font("Montserrat", 8.999999F);
            bt_remove.Location = new Point(325, 227);
            bt_remove.Name = "bt_remove";
            bt_remove.Size = new Size(86, 25);
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
            tx_sample.Font = new Font("Montserrat", 8.999999F);
            tx_sample.Location = new Point(222, 227);
            tx_sample.Name = "tx_sample";
            tx_sample.Size = new Size(69, 20);
            tx_sample.TabIndex = 10;
            tx_sample.Text = "SampleID";
            // 
            // tb_sample
            // 
            tb_sample.Font = new Font("Montserrat", 8.999999F);
            tb_sample.Location = new Point(269, 227);
            tb_sample.MaxLength = 3;
            tb_sample.Name = "tb_sample";
            tb_sample.Size = new Size(46, 22);
            tb_sample.TabIndex = 11;
            tb_sample.TextAlign = HorizontalAlignment.Right;
            tb_sample.TextChanged += tb_sample_TextChanged;
            // 
            // bt_returnSample
            // 
            bt_returnSample.Enabled = false;
            bt_returnSample.FlatStyle = FlatStyle.Popup;
            bt_returnSample.Font = new Font("Segoe UI Symbol", 8.25F, FontStyle.Bold);
            bt_returnSample.Location = new Point(222, 245);
            bt_returnSample.Name = "bt_returnSample";
            bt_returnSample.Size = new Size(27, 23);
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
            bt_nextSample.Location = new Point(247, 245);
            bt_nextSample.Name = "bt_nextSample";
            bt_nextSample.Size = new Size(27, 23);
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
            bt_modify.Font = new Font("Montserrat", 8.999999F);
            bt_modify.Location = new Point(313, 273);
            bt_modify.Name = "bt_modify";
            bt_modify.Size = new Size(97, 25);
            bt_modify.TabIndex = 14;
            bt_modify.Text = "Modify";
            bt_modify.UseVisualStyleBackColor = true;
            bt_modify.Click += bt_modify_Click;
            // 
            // tx_sampleCount
            // 
            tx_sampleCount.AutoSize = true;
            tx_sampleCount.Location = new Point(274, 250);
            tx_sampleCount.Name = "tx_sampleCount";
            tx_sampleCount.Size = new Size(0, 21);
            tx_sampleCount.TabIndex = 15;
            tx_sampleCount.UseCompatibleTextRendering = true;
            // 
            // tc_modify
            // 
            tc_modify.Appearance = TabAppearance.Buttons;
            tc_modify.Controls.Add(tp_user);
            tc_modify.Controls.Add(tp_sample);
            tc_modify.Font = new Font("Montserrat", 8.999999F);
            tc_modify.Location = new Point(185, 273);
            tc_modify.Name = "tc_modify";
            tc_modify.SelectedIndex = 0;
            tc_modify.Size = new Size(228, 161);
            tc_modify.TabIndex = 16;
            tc_modify.Tag = "";
            tc_modify.Visible = false;
            // 
            // tp_user
            // 
            tp_user.BorderStyle = BorderStyle.FixedSingle;
            tp_user.Controls.Add(tx_selectedID);
            tp_user.Controls.Add(bt_saveAlterUser);
            tp_user.Controls.Add(tb_alterName);
            tp_user.Font = new Font("Nirmala UI", 9F);
            tp_user.Location = new Point(4, 30);
            tp_user.Name = "tp_user";
            tp_user.Padding = new Padding(3);
            tp_user.Size = new Size(220, 127);
            tp_user.TabIndex = 0;
            tp_user.Text = "User";
            tp_user.UseVisualStyleBackColor = true;
            // 
            // tx_selectedID
            // 
            tx_selectedID.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            tx_selectedID.AutoSize = true;
            tx_selectedID.Font = new Font("Montserrat", 8.999999F);
            tx_selectedID.Location = new Point(68, 16);
            tx_selectedID.Name = "tx_selectedID";
            tx_selectedID.Size = new Size(87, 18);
            tx_selectedID.TabIndex = 6;
            tx_selectedID.Text = "Selected ID - ";
            tx_selectedID.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // bt_saveAlterUser
            // 
            bt_saveAlterUser.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            bt_saveAlterUser.Font = new Font("Montserrat", 8.999999F);
            bt_saveAlterUser.Location = new Point(71, 92);
            bt_saveAlterUser.Name = "bt_saveAlterUser";
            bt_saveAlterUser.Size = new Size(75, 23);
            bt_saveAlterUser.TabIndex = 4;
            bt_saveAlterUser.Text = "Save";
            bt_saveAlterUser.UseVisualStyleBackColor = true;
            bt_saveAlterUser.Click += bt_saveAlterUser_Click;
            // 
            // tb_alterName
            // 
            tb_alterName.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            tb_alterName.Font = new Font("Montserrat", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_alterName.Location = new Point(51, 52);
            tb_alterName.Name = "tb_alterName";
            tb_alterName.PlaceholderText = "New Name";
            tb_alterName.Size = new Size(114, 22);
            tb_alterName.TabIndex = 1;
            tb_alterName.TextAlign = HorizontalAlignment.Center;
            // 
            // tp_sample
            // 
            tp_sample.BorderStyle = BorderStyle.FixedSingle;
            tp_sample.Controls.Add(bt_sampleReplace);
            tp_sample.Controls.Add(tx_selectedIDSample);
            tp_sample.Controls.Add(bt_saveAlterSample);
            tp_sample.Location = new Point(4, 30);
            tp_sample.Name = "tp_sample";
            tp_sample.Padding = new Padding(3);
            tp_sample.Size = new Size(220, 127);
            tp_sample.TabIndex = 1;
            tp_sample.Text = "Sample";
            tp_sample.UseVisualStyleBackColor = true;
            // 
            // bt_sampleReplace
            // 
            bt_sampleReplace.BackgroundImage = Properties.Resources.fingerprint_scan;
            bt_sampleReplace.BackgroundImageLayout = ImageLayout.Zoom;
            bt_sampleReplace.FlatAppearance.BorderColor = SystemColors.Control;
            bt_sampleReplace.FlatAppearance.BorderSize = 0;
            bt_sampleReplace.FlatAppearance.MouseOverBackColor = SystemColors.MenuHighlight;
            bt_sampleReplace.FlatStyle = FlatStyle.Flat;
            bt_sampleReplace.Location = new Point(88, 41);
            bt_sampleReplace.Name = "bt_sampleReplace";
            bt_sampleReplace.Size = new Size(38, 38);
            bt_sampleReplace.TabIndex = 9;
            bt_sampleReplace.UseVisualStyleBackColor = true;
            bt_sampleReplace.Click += bt_sampleReplace_Click;
            // 
            // tx_selectedIDSample
            // 
            tx_selectedIDSample.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            tx_selectedIDSample.AutoSize = true;
            tx_selectedIDSample.Font = new Font("Montserrat", 8.999999F);
            tx_selectedIDSample.Location = new Point(68, 16);
            tx_selectedIDSample.Name = "tx_selectedIDSample";
            tx_selectedIDSample.Size = new Size(87, 18);
            tx_selectedIDSample.TabIndex = 8;
            tx_selectedIDSample.Text = "Selected ID - ";
            tx_selectedIDSample.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // bt_saveAlterSample
            // 
            bt_saveAlterSample.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            bt_saveAlterSample.Font = new Font("Montserrat", 8.999999F);
            bt_saveAlterSample.Location = new Point(71, 92);
            bt_saveAlterSample.Name = "bt_saveAlterSample";
            bt_saveAlterSample.Size = new Size(75, 23);
            bt_saveAlterSample.TabIndex = 7;
            bt_saveAlterSample.Text = "Save";
            bt_saveAlterSample.UseVisualStyleBackColor = true;
            bt_saveAlterSample.Click += bt_saveAlterSample_Click;
            // 
            // flp_devices
            // 
            flp_devices.Location = new Point(3, 266);
            flp_devices.Name = "flp_devices";
            flp_devices.Size = new Size(32, 164);
            flp_devices.TabIndex = 17;
            // 
            // tx_deviceName
            // 
            tx_deviceName.AutoSize = true;
            tx_deviceName.Font = new Font("Montserrat", 6.999999F);
            tx_deviceName.Location = new Point(3, 3);
            tx_deviceName.Name = "tx_deviceName";
            tx_deviceName.Size = new Size(43, 16);
            tx_deviceName.TabIndex = 0;
            tx_deviceName.Text = "Device:";
            // 
            // pn_deviceInf
            // 
            pn_deviceInf.BackColor = Color.LightSkyBlue;
            pn_deviceInf.Controls.Add(cb_autoOn);
            pn_deviceInf.Controls.Add(tx_autoOn);
            pn_deviceInf.Controls.Add(tb_serialN);
            pn_deviceInf.Controls.Add(tx_serialNumber);
            pn_deviceInf.Controls.Add(tb_deviceName);
            pn_deviceInf.Controls.Add(tx_deviceName);
            pn_deviceInf.Font = new Font("Montserrat", 7.999999F);
            pn_deviceInf.Location = new Point(34, 269);
            pn_deviceInf.Name = "pn_deviceInf";
            pn_deviceInf.Size = new Size(150, 160);
            pn_deviceInf.TabIndex = 19;
            // 
            // cb_autoOn
            // 
            cb_autoOn.AutoSize = true;
            cb_autoOn.BackColor = Color.LightSkyBlue;
            cb_autoOn.FlatAppearance.BorderColor = Color.LightSkyBlue;
            cb_autoOn.FlatAppearance.BorderSize = 4;
            cb_autoOn.FlatAppearance.CheckedBackColor = Color.DeepSkyBlue;
            cb_autoOn.FlatAppearance.MouseDownBackColor = Color.SkyBlue;
            cb_autoOn.FlatAppearance.MouseOverBackColor = Color.LightBlue;
            cb_autoOn.FlatStyle = FlatStyle.System;
            cb_autoOn.Location = new Point(111, 35);
            cb_autoOn.Name = "cb_autoOn";
            cb_autoOn.Size = new Size(25, 13);
            cb_autoOn.TabIndex = 5;
            cb_autoOn.UseVisualStyleBackColor = true;
            cb_autoOn.CheckedChanged += cb_autoOn_CheckedChanged;
            // 
            // tx_autoOn
            // 
            tx_autoOn.AutoSize = true;
            tx_autoOn.Font = new Font("Montserrat", 6.999999F);
            tx_autoOn.Location = new Point(3, 35);
            tx_autoOn.Name = "tx_autoOn";
            tx_autoOn.Size = new Size(49, 16);
            tx_autoOn.TabIndex = 4;
            tx_autoOn.Text = "Auto On";
            // 
            // tb_serialN
            // 
            tb_serialN.BackColor = Color.LightSkyBlue;
            tb_serialN.BorderStyle = BorderStyle.None;
            tb_serialN.Font = new Font("Montserrat", 6.999999F);
            tb_serialN.Location = new Point(22, 20);
            tb_serialN.Name = "tb_serialN";
            tb_serialN.ReadOnly = true;
            tb_serialN.Size = new Size(115, 12);
            tb_serialN.TabIndex = 3;
            tb_serialN.TextAlign = HorizontalAlignment.Right;
            // 
            // tx_serialNumber
            // 
            tx_serialNumber.AutoSize = true;
            tx_serialNumber.Font = new Font("Montserrat", 6.999999F);
            tx_serialNumber.Location = new Point(3, 20);
            tx_serialNumber.Name = "tx_serialNumber";
            tx_serialNumber.Size = new Size(23, 16);
            tx_serialNumber.TabIndex = 2;
            tx_serialNumber.Text = "SN:";
            // 
            // tb_deviceName
            // 
            tb_deviceName.BackColor = Color.LightSkyBlue;
            tb_deviceName.BorderStyle = BorderStyle.None;
            tb_deviceName.Font = new Font("Montserrat", 9.999999F);
            tb_deviceName.Location = new Point(49, 0);
            tb_deviceName.Name = "tb_deviceName";
            tb_deviceName.ReadOnly = true;
            tb_deviceName.Size = new Size(88, 17);
            tb_deviceName.TabIndex = 1;
            tb_deviceName.TextAlign = HorizontalAlignment.Right;
            // 
            // tx_NBioV
            // 
            tx_NBioV.AutoSize = true;
            tx_NBioV.Font = new Font("Montserrat", 6.999999F);
            tx_NBioV.Location = new Point(12, 432);
            tx_NBioV.Name = "tx_NBioV";
            tx_NBioV.Size = new Size(79, 16);
            tx_NBioV.TabIndex = 20;
            tx_NBioV.Text = "NBioVersion - ";
            // 
            // CRUD
            // 
            AutoScaleDimensions = new SizeF(8F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(760, 446);
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
            Controls.Add(pn_deviceInf);
            Controls.Add(flp_devices);
            Controls.Add(tx_NBioV);
            Font = new Font("Montserrat", 8.999999F);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "CRUD";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CRUD";
            ((ISupportInitialize)pb_actvatedFir).EndInit();
            ((ISupportInitialize)dg_users).EndInit();
            ((ISupportInitialize)pb_selectedFir).EndInit();
            tc_modify.ResumeLayout(false);
            tp_user.ResumeLayout(false);
            tp_user.PerformLayout();
            tp_sample.ResumeLayout(false);
            tp_sample.PerformLayout();
            pn_deviceInf.ResumeLayout(false);
            pn_deviceInf.PerformLayout();
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
        private TextBox tb_alterName;
        private Button bt_saveAlterUser;
        private Label tx_selectedID;
        private Label tx_selectedIDSample;
        private Button bt_saveAlterSample;
        private Button bt_sampleReplace;
        private FlowLayoutPanel flp_devices;
        private Label tx_deviceName;
        private Panel pn_deviceInf;
        private Label tx_serialNumber;
        private TextBox tb_deviceName;
        private TextBox tb_serialN;
        private CheckBox cb_autoOn;
        private Label tx_autoOn;
        private BackgroundWorker fingerCheckWorker;
        private Label tx_NBioV;
    }
}
