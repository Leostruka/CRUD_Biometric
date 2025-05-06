using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Biometric.DataAccess
{
    internal class SQLite
    {
        private readonly string _dbPath = "local_database.db";

        public SQLite()
        {
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
                InitializeDatabase();
            }
        }

        private void InitializeDatabase()
        {
            using var connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            connection.Open();

            string createUserTable = @"
                CREATE TABLE IF NOT EXISTS user (
                  id INTEGER PRIMARY KEY,
                  name TEXT NOT NULL DEFAULT ''
                );";

            string createFirTable = @"
                CREATE TABLE IF NOT EXISTS fir (
                  id INTEGER NOT NULL,
                  hash TEXT NOT NULL,
                  sample INTEGER NOT NULL DEFAULT 0,
                  PRIMARY KEY (id, sample),
                  FOREIGN KEY (id) REFERENCES user (id) ON DELETE CASCADE
                );";

            string createAuditTable = @"
                CREATE TABLE IF NOT EXISTS auditdata (
                  id INTEGER,
                  data TEXT,
                  imageWidth INTEGER,
                  imageHeight INTEGER,
                  sample INTEGER,
                  PRIMARY KEY (id, sample),
                  FOREIGN KEY (id) REFERENCES user (id) ON DELETE CASCADE
                );";

            string createIndexes = @"
                CREATE INDEX IF NOT EXISTS idx_fir_id ON fir (id);
                CREATE INDEX IF NOT EXISTS idx_fir_sample ON fir (sample);
                CREATE INDEX IF NOT EXISTS idx_auditdata_id ON auditdata (id);
                CREATE INDEX IF NOT EXISTS idx_auditdata_sample ON auditdata (sample);
            ";

            // Enable foreign key support
            string enableForeignKeys = "PRAGMA foreign_keys = ON;";

            using var command = new SQLiteCommand(createUserTable + createFirTable + createAuditTable + createIndexes + enableForeignKeys, connection);
            command.ExecuteNonQuery();
        }

        public SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;Foreign Keys=True;");
            return connection;
        }
        
        public void ImportFromMySQL(Connection mysqlConnection)
        {
            // Implementação futura para importar dados do MySQL quando disponível
            // Isso pode ser usado para sincronizar os bancos de dados quando a internet voltar
        }
    }
}
