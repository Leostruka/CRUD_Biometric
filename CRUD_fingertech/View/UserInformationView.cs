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
        string name = "";

        public UserInformationView()
        {
            InitializeComponent();

            // Clear tb_name
            tb_name.Clear();
        }

        public string GetName()
        {
            return tb_name.Text;
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
            }
        }
    }
}
