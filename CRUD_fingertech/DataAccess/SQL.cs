using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_User.DataAccess
{
    public class SQL
    {
        MySqlCommand sql;

        // Method to execute SQL commands
        public void SQLConnection(string cmd)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand(cmd, con.con);
            sql.ExecuteNonQuery();
            con.CloseConnection();
        }

        // Method to insert data into the database


        // Method to execute SQL commands and return a value
    }
}
