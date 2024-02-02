using CRUD_User.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
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
        public DataTable InsertData(UserModel.User user, FIRModel.FIR fir)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("INSERT INTO fir (id, hash, sample) values(@name, @hash, @sample)", con.con);
            sql.Parameters.AddWithValue("@name", fir.id);
            sql.Parameters.AddWithValue("@hash", fir.hash);

            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }

        // Method to execute SQL commands and return a value
    }
}
