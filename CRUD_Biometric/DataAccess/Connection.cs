using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Data.SQLite;
using System.Net.NetworkInformation;
using System.Data;

namespace CRUD_Biometric.DataAccess
{
    internal class Connection
    {
        readonly string mysqlConnection = "SERVER=localhost; DATABASE=crud_users_db; UID=root; PASSWORD=";
        public MySqlConnection mysqlCon = null;
        public SQLiteConnection sqliteCon = null;
        private bool useMySQL;
        private readonly SQLite sqliteHelper = new SQLite();

        public bool CheckInternetConnection()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("8.8.8.8", 2000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        public void OpenConnection()
        {
            try
            {
                if (useMySQL)
                {
                    mysqlCon = new MySqlConnection(mysqlConnection);
                    mysqlCon.Open();
                }
                else
                {
                    sqliteCon = sqliteHelper.GetConnection();
                    sqliteCon.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error: " + ex.Message);
                
                // If MySQL connection fails, try SQLite as fallback
                if (useMySQL)
                {
                    try
                    {
                        useMySQL = false;
                        sqliteCon = sqliteHelper.GetConnection();
                        sqliteCon.Open();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("SQLite Fallback Error: " + ex2.Message);
                    }
                }
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (useMySQL && mysqlCon != null && mysqlCon.State == System.Data.ConnectionState.Open)
                {
                    mysqlCon.Close();
                }
                else if (!useMySQL && sqliteCon != null && sqliteCon.State == System.Data.ConnectionState.Open)
                {
                    sqliteCon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Close connection Error: " + ex.Message);
            }
        }

        public bool IsUsingMySQL()
        {
            return useMySQL;
        }
        
        public IDbConnection GetActiveConnection()
        {
            if (useMySQL)
                return mysqlCon;
            else
                return sqliteCon;
        }
    }
}
