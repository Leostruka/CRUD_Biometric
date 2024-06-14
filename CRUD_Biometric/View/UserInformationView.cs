using CRUD_User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_User.View
{
    public partial class UserInformationView : Form
    {
        public UserInformationView()
        {
            InitializeComponent();

            // Clear tb_name
            tb_name.Clear();
            tb_name.Focus();

            // Attach event handler to handle Enter key press on tb_name
            tb_name.KeyDown += Tb_name_KeyDown;
        }

        public string GetName()
        {
            return tb_name.Text;
        }

        public void SetName(string name)
        {
            tb_name.Text = name;
        }

        private void bt_confirmName_Click(object sender, EventArgs e)
        {
            if (tb_name.Text != "")
            {
                // Close form
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a name.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tb_name.Focus();
            }
        }

        private void Tb_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bt_confirmName_Click(sender, e);
            }
        }

        private void X_Click(object sender, EventArgs e)
        {
            tb_name.Text = "Cancel";
            this.Close();
        }
    }
}
