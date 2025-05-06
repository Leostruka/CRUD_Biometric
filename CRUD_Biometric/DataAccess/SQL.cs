using CRUD_Biometric.Model;
using MySqlConnector;
using System.Data;
using System.Data.SQLite;

namespace CRUD_Biometric.DataAccess
{
    public class SQL
    {
        MySqlCommand mysql_sql;
        SQLiteCommand sqlite_sql;
        private Connection connection;

        public SQL()
        {
            connection = new Connection();
        }

        // Method to insert data into the user database
        public DataTable InsertDataUser(UserModel.User user)
        {
            connection.OpenConnection();
            DataTable dt = new DataTable();

            try
            {
                if (connection.IsUsingMySQL())
                {
                    mysql_sql = new MySqlCommand("INSERT INTO user (id, name) values(@id, @name)", connection.mysqlCon);
                    mysql_sql.Parameters.AddWithValue("@id", user.id);
                    mysql_sql.Parameters.AddWithValue("@name", char.ToUpper(user.name[0]) + user.name.Substring(1));

                    MySqlDataAdapter da = new MySqlDataAdapter(mysql_sql);
                    da.Fill(dt);
                }
                else
                {
                    sqlite_sql = new SQLiteCommand("INSERT INTO user (id, name) values(@id, @name)", connection.sqliteCon);
                    sqlite_sql.Parameters.AddWithValue("@id", user.id);
                    sqlite_sql.Parameters.AddWithValue("@name", char.ToUpper(user.name[0]) + user.name.Substring(1));

                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                }
            }
            finally
            {
                connection.CloseConnection();
            }
            return dt;
        }
        
        // Method to insert data into the fir database
        public DataTable InsertDataFir(FIRModel.FIR fir)
        {
            connection.OpenConnection();
            DataTable dt = new DataTable();

            try
            {
                if (connection.IsUsingMySQL())
                {
                    mysql_sql = new MySqlCommand("INSERT INTO fir (id, hash, sample) values(@id, @hash, @sample)", connection.mysqlCon);
                    mysql_sql.Parameters.AddWithValue("@id", fir.id);
                    mysql_sql.Parameters.AddWithValue("@hash", fir.hash);
                    mysql_sql.Parameters.AddWithValue("@sample", fir.sample);

                    MySqlDataAdapter da = new MySqlDataAdapter(mysql_sql);
                    da.Fill(dt);
                }
                else
                {
                    sqlite_sql = new SQLiteCommand("INSERT INTO fir (id, hash, sample) values(@id, @hash, @sample)", connection.sqliteCon);
                    sqlite_sql.Parameters.AddWithValue("@id", fir.id);
                    sqlite_sql.Parameters.AddWithValue("@hash", fir.hash);
                    sqlite_sql.Parameters.AddWithValue("@sample", fir.sample);

                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                }
            }
            finally
            {
                connection.CloseConnection();
            }
            return dt;
        }

        // Method to insert data into the auditdata database
        public DataTable InsertDataAudit(AuditModel.Audit audit)
        {
            connection.OpenConnection();
            DataTable dt = new DataTable();

            try
            {
                // Convert audit.data to a hexadecimal string
                string hexData = BitConverter.ToString(audit.data).Replace("-", string.Empty);
                
                if (connection.IsUsingMySQL())
                {
                    mysql_sql = new MySqlCommand("INSERT INTO auditdata (id, data, imageWidth, imageHeight, sample) values(@id, @data, @imageWidth, @imageHeight, @sample)", connection.mysqlCon);
                    mysql_sql.Parameters.AddWithValue("@id", audit.id);
                    mysql_sql.Parameters.AddWithValue("@data", hexData);
                    mysql_sql.Parameters.AddWithValue("@imageWidth", audit.imageWidth);
                    mysql_sql.Parameters.AddWithValue("@imageHeight", audit.imageHeight);
                    mysql_sql.Parameters.AddWithValue("@sample", audit.sample);

                    MySqlDataAdapter da = new MySqlDataAdapter(mysql_sql);
                    da.Fill(dt);
                }
                else
                {
                    sqlite_sql = new SQLiteCommand("INSERT INTO auditdata (id, data, imageWidth, imageHeight, sample) values(@id, @data, @imageWidth, @imageHeight, @sample)", connection.sqliteCon);
                    sqlite_sql.Parameters.AddWithValue("@id", audit.id);
                    sqlite_sql.Parameters.AddWithValue("@data", hexData);
                    sqlite_sql.Parameters.AddWithValue("@imageWidth", audit.imageWidth);
                    sqlite_sql.Parameters.AddWithValue("@imageHeight", audit.imageHeight);
                    sqlite_sql.Parameters.AddWithValue("@sample", audit.sample);

                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                }
            }
            finally
            {
                connection.CloseConnection();
            }
            return dt;
        }

        // Method to return fir's
        public DataTable GetDataFir()
        {
            connection.OpenConnection();
            DataTable dt = new DataTable();

            try
            {
                if (connection.IsUsingMySQL())
                {
                    mysql_sql = new MySqlCommand("SELECT * FROM fir " +
                                           "ORDER BY fir.id, fir.sample ASC", connection.mysqlCon);
                    MySqlDataAdapter da = new MySqlDataAdapter(mysql_sql);
                    da.Fill(dt);
                }
                else
                {
                    sqlite_sql = new SQLiteCommand("SELECT * FROM fir " +
                                             "ORDER BY fir.id, fir.sample ASC", connection.sqliteCon);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                }
            }
            finally
            {
                connection.CloseConnection();
            }
            return dt;
        }

        // Method to return user's and fir's data from the database
        public DataTable GetDataUserFir()
        {
            connection.OpenConnection();
            DataTable dt = new DataTable();

            try
            {
                if (connection.IsUsingMySQL())
                {
                    mysql_sql = new MySqlCommand("SELECT user.id, user.name, fir.hash, fir.sample FROM user " +
                                           "INNER JOIN fir ON user.id = fir.id " +
                                           "ORDER BY fir.id, fir.sample ASC", connection.mysqlCon);
                    MySqlDataAdapter da = new MySqlDataAdapter(mysql_sql);
                    da.Fill(dt);
                }
                else
                {
                    sqlite_sql = new SQLiteCommand("SELECT user.id, user.name, fir.hash, fir.sample FROM user " +
                                             "INNER JOIN fir ON user.id = fir.id " +
                                             "ORDER BY fir.id, fir.sample ASC", connection.sqliteCon);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                }
            }
            finally
            {
                connection.CloseConnection();
            }
            return dt;
        }

        // Method to get specific user, fir and auditdata data from the database
        public DataTable GetSpecificDataAudit(int id)
        {
            connection.OpenConnection();
            DataTable dt = new DataTable();

            try
            {
                if (connection.IsUsingMySQL())
                {
                    mysql_sql = new MySqlCommand("SELECT id, data, imageWidth, imageHeight, sample FROM auditdata " +
                                           "WHERE id = @id " +
                                           "ORDER BY id, sample ASC", connection.mysqlCon);
                    mysql_sql.Parameters.AddWithValue("@id", id);
                    MySqlDataAdapter da = new MySqlDataAdapter(mysql_sql);
                    da.Fill(dt);
                }
                else
                {
                    sqlite_sql = new SQLiteCommand("SELECT id, data, imageWidth, imageHeight, sample FROM auditdata " +
                                             "WHERE id = @id " +
                                             "ORDER BY id, sample ASC", connection.sqliteCon);
                    sqlite_sql.Parameters.AddWithValue("@id", id);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                }
            }
            finally
            {
                connection.CloseConnection();
            }
            return dt;
        }

        // Method to alter data in the User database
        public void UpdateDataUser(UserModel.User user)
        {
            connection.OpenConnection();
            DataTable dt = new DataTable();

            try
            {
                if (connection.IsUsingMySQL())
                {
                    mysql_sql = new MySqlCommand("UPDATE user SET name = @name WHERE id = @id", connection.mysqlCon);
                    mysql_sql.Parameters.AddWithValue("@id", user.id);
                    mysql_sql.Parameters.AddWithValue("@name", char.ToUpper(user.name[0]) + user.name.Substring(1));
                    MySqlDataAdapter da = new MySqlDataAdapter(mysql_sql);
                    da.Fill(dt);
                }
                else
                {
                    sqlite_sql = new SQLiteCommand("UPDATE user SET name = @name WHERE id = @id", connection.sqliteCon);
                    sqlite_sql.Parameters.AddWithValue("@id", user.id);
                    sqlite_sql.Parameters.AddWithValue("@name", char.ToUpper(user.name[0]) + user.name.Substring(1));
                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                }
            }
            finally
            {
                connection.CloseConnection();
            }
        }

        // Method to alter data in the Fir and Sample database
        public void UpdateDataFirAudit(FIRModel.FIR fir, AuditModel.Audit audit)
        {
            connection.OpenConnection();
            DataTable dt = new DataTable();

            try
            {
                // Convert audit.data to a hexadecimal string
                string hexData = BitConverter.ToString(audit.data).Replace("-", string.Empty);
                
                if (connection.IsUsingMySQL())
                {
                    mysql_sql = new MySqlCommand("UPDATE fir SET hash = @hash WHERE id = @id AND sample = @sample; " +
                                           "UPDATE auditdata SET data = @data, imageWidth = @imageWidth, imageHeight = @imageHeight WHERE id = @id AND sample = @sample;", connection.mysqlCon);
                    mysql_sql.Parameters.AddWithValue("@id", fir.id);
                    mysql_sql.Parameters.AddWithValue("@hash", fir.hash);
                    mysql_sql.Parameters.AddWithValue("@sample", fir.sample);
                    mysql_sql.Parameters.AddWithValue("@data", hexData);
                    mysql_sql.Parameters.AddWithValue("@imageWidth", audit.imageWidth);
                    mysql_sql.Parameters.AddWithValue("@imageHeight", audit.imageHeight);

                    MySqlDataAdapter da = new MySqlDataAdapter(mysql_sql);
                    da.Fill(dt);
                }
                else
                {
                    // SQLite doesn't support multiple statements in a single command, so we need to execute two separate commands
                    sqlite_sql = new SQLiteCommand("UPDATE fir SET hash = @hash WHERE id = @id AND sample = @sample", connection.sqliteCon);
                    sqlite_sql.Parameters.AddWithValue("@id", fir.id);
                    sqlite_sql.Parameters.AddWithValue("@hash", fir.hash);
                    sqlite_sql.Parameters.AddWithValue("@sample", fir.sample);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                    
                    // Second command for updating auditdata
                    sqlite_sql = new SQLiteCommand("UPDATE auditdata SET data = @data, imageWidth = @imageWidth, imageHeight = @imageHeight WHERE id = @id AND sample = @sample", connection.sqliteCon);
                    sqlite_sql.Parameters.AddWithValue("@id", fir.id);
                    sqlite_sql.Parameters.AddWithValue("@data", hexData);
                    sqlite_sql.Parameters.AddWithValue("@imageWidth", audit.imageWidth);
                    sqlite_sql.Parameters.AddWithValue("@imageHeight", audit.imageHeight);
                    sqlite_sql.Parameters.AddWithValue("@sample", fir.sample);
                    da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                }
            }
            finally
            {
                connection.CloseConnection();
            }
        }

        // Method to Delete data from the User database
        public DataTable DeleteDataUser(int id)
        {
            connection.OpenConnection();
            DataTable dt = new DataTable();

            try
            {
                if (connection.IsUsingMySQL())
                {
                    mysql_sql = new MySqlCommand("DELETE FROM user WHERE id = @id", connection.mysqlCon);
                    mysql_sql.Parameters.AddWithValue("@id", id);
                    MySqlDataAdapter da = new MySqlDataAdapter(mysql_sql);
                    da.Fill(dt);
                }
                else
                {
                    sqlite_sql = new SQLiteCommand("DELETE FROM user WHERE id = @id", connection.sqliteCon);
                    sqlite_sql.Parameters.AddWithValue("@id", id);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                }
            }
            finally
            {
                connection.CloseConnection();
            }
            return dt;
        }

        // Method to Delete data from the Fir and Auditdata database
        public DataTable DeleteDataFirAudit(int id, int sample)
        {
            connection.OpenConnection();
            DataTable dt = new DataTable();

            try
            {
                if (connection.IsUsingMySQL())
                {
                    mysql_sql = new MySqlCommand("DELETE FROM fir WHERE id = @id AND sample = @sample; " +
                                         "DELETE FROM auditdata WHERE id = @id AND sample = @sample;", connection.mysqlCon);
                    mysql_sql.Parameters.AddWithValue("@id", id);
                    mysql_sql.Parameters.AddWithValue("@sample", sample);
                    MySqlDataAdapter da = new MySqlDataAdapter(mysql_sql);
                    da.Fill(dt);
                }
                else
                {
                    // SQLite doesn't support multiple statements in a single command, so we need to execute two separate commands
                    sqlite_sql = new SQLiteCommand("DELETE FROM fir WHERE id = @id AND sample = @sample", connection.sqliteCon);
                    sqlite_sql.Parameters.AddWithValue("@id", id);
                    sqlite_sql.Parameters.AddWithValue("@sample", sample);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                    
                    // Second command for deleting from auditdata
                    sqlite_sql = new SQLiteCommand("DELETE FROM auditdata WHERE id = @id AND sample = @sample", connection.sqliteCon);
                    sqlite_sql.Parameters.AddWithValue("@id", id);
                    sqlite_sql.Parameters.AddWithValue("@sample", sample);
                    da = new SQLiteDataAdapter(sqlite_sql);
                    da.Fill(dt);
                }
            }
            finally
            {
                connection.CloseConnection();
            }
            return dt;
        }
    }
}
