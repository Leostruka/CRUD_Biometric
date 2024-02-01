using CRUD_fingertech;
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
        }

        private void bt_confirmName_Click(object sender, EventArgs e)
        {
            if (tb_name.Text != "")
            {
                // Set user.name to tb_name
                

                // Close form
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a name.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
