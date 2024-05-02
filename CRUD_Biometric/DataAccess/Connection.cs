using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace CRUD_User.DataAccess
{
    internal class Connection
    {
        readonly string connection = "SERVER=localhost; DATABASE=crud_users_db; UID=root; PASSWORD=";
        public MySqlConnection con = null;

        public void OpenConnection()
        {
            try
            {
                con = new MySqlConnection(connection);
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error" + ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Close connection Error" + ex.Message);
            }
        }
    }
}
