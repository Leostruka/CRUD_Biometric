using CRUD_Biometric.Model;
using MySqlConnector;
using System.Data;

namespace CRUD_Biometric.DataAccess
{
    public class SQL
    {
        MySqlCommand sql;

        // Method to insert data into the user database
        public DataTable InsertDataUser(UserModel.User user)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("INSERT INTO user (id, name) values(@id, @name)", con.con);
            sql.Parameters.AddWithValue("@id", user.id);
            sql.Parameters.AddWithValue("@name", char.ToUpper(user.name[0]) + user.name.Substring(1));

            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            return dt;
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

        // Method to insert data into the auditdata database
        public DataTable InsertDataAudit(AuditModel.Audit audit)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("INSERT INTO auditdata (id, data, imageWidth, imageHeight, sample) values(@id, @data, @imageWidth, @imageHeight, @sample)", con.con);
            sql.Parameters.AddWithValue("@id", audit.id);

            // Convert audit.data to a hexadecimal string
            string hexData = BitConverter.ToString(audit.data).Replace("-", string.Empty);
            sql.Parameters.AddWithValue("@data", hexData);
            sql.Parameters.AddWithValue("@imageWidth", audit.imageWidth);
            sql.Parameters.AddWithValue("@imageHeight", audit.imageHeight);
            sql.Parameters.AddWithValue("@sample", audit.sample);

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
            sql = new MySqlCommand("SELECT * FROM fir " +
                                   "ORDER BY fir.id, fir.sample ASC", con.con);
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
            sql = new MySqlCommand("SELECT user.id, user.name, fir.hash, fir.sample FROM user " +
                                   "INNER JOIN fir ON user.id = fir.id " +
                                   "ORDER BY fir.id, fir.sample ASC", con.con);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }

        // Method to get specific user, fir and auditdata data from the database
        public DataTable GetSpecificDataAudit(int id)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("SELECT id, data, imageWidth, imageHeight, sample FROM auditdata " +
                                   "WHERE id = @id " +
                                   "ORDER BY id, sample ASC", con.con);
            sql.Parameters.AddWithValue("@id", id);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }

        // Method to alter data in the User database
        public void UpdateDataUser(UserModel.User user)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("UPDATE user SET name = @name WHERE id = @id", con.con);
            sql.Parameters.AddWithValue("@id", user.id);
            sql.Parameters.AddWithValue("@name", char.ToUpper(user.name[0]) + user.name.Substring(1));
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
        }

        // Method to alter data in the Fir and Sample database
        public void UpdateDataFirAudit(FIRModel.FIR fir, AuditModel.Audit audit)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("UPDATE fir SET hash = @hash WHERE id = @id AND sample = @sample; " +
                                   "UPDATE auditdata SET data = @data, imageWidth = @imageWidth, imageHeight = @imageHeight WHERE id = @id AND sample = @sample;", con.con);
            sql.Parameters.AddWithValue("@id", fir.id);
            sql.Parameters.AddWithValue("@hash", fir.hash);
            sql.Parameters.AddWithValue("@sample", fir.sample);

            // Convert audit.data to a hexadecimal string
            string hexData = BitConverter.ToString(audit.data).Replace("-", string.Empty);
            sql.Parameters.AddWithValue("@data", hexData);
            sql.Parameters.AddWithValue("@imageWidth", audit.imageWidth);
            sql.Parameters.AddWithValue("@imageHeight", audit.imageHeight);

            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
        }

        // Method to Delete data from the User database
        public DataTable DeleteDataUser(int id)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("DELETE FROM user WHERE id = @id", con.con);
            sql.Parameters.AddWithValue("@id", id);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }

        // Method to Delete data from the Fir and Auditdata database
        public DataTable DeleteDataFirAudit(int id, int sample)
        {
            Connection con = new Connection();
            con.OpenConnection();
            sql = new MySqlCommand("DELETE FROM fir WHERE id = @id AND sample = @sample; " +
                                   "DELETE FROM auditdata WHERE id = @id AND sample = @sample;", con.con);
            sql.Parameters.AddWithValue("@id", id);
            sql.Parameters.AddWithValue("@sample", sample);
            MySqlDataAdapter da = new MySqlDataAdapter(sql);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }
    }
}
