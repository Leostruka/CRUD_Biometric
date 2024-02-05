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

        // Method to insert data into the fir database
        public DataTable InsertDataFir(FIRModel.FIR fir)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("INSERT INTO fir (id, hash, sample) values(@id, @hash, @sample)", con.con);
            sql.Parameters.AddWithValue("@id", fir.id);
            sql.Parameters.AddWithValue("@hash", fir.hash);
            sql.Parameters.AddWithValue("@sample", fir.sample);

            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }

        // Method to insert data into the user database
        public DataTable InsertDataUser(UserModel.User user)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("INSERT INTO user (id, name) values(@id, @name)", con.con);
            sql.Parameters.AddWithValue("@id", user.id);
            sql.Parameters.AddWithValue("@name", user.name);

            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }

        // Method to return user's and fir's data from the database
        public DataTable GetDataUserFir()
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("SELECT user.id, user.name, fir.id, fir.hash, fir.sample FROM user " +
                                   "INNER JOIN fir ON user.id = fir.id", con.con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }

        // Method to return fir's
        public DataTable GetDataFir()
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("SELECT * FROM fir", con.con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }
    }
}
