using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;

namespace IExpro.Infrastructure.Services
{
    public class BackupService
    {
        private readonly string _connectionString;
        private readonly string _backupFolderFullPath;
        private readonly string[] _systemDatabaseNames = { "master", "tempdb", "model", "msdb" };

        public BackupService(string connectionString, string backupFolderFullPath)
        {
            _connectionString = connectionString;
            _backupFolderFullPath = backupFolderFullPath;
        }

        public void BackupAllUserDatabases()
        {
            foreach (string databaseName in GetAllUserDatabases())
            {
                BackupDatabase(databaseName);
            }
        }

        public void BackupDatabase(string databaseName)
        {
            string filePath = BuildBackupPathWithFilename(databaseName);

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"BACKUP DATABASE @DatabaseName TO DISK=@BackupPath
INSERT INTO [dbo].[tblDatabaseBackup]([DatabaseName],[BackupPath],[CreatedOn],[CreatedBy])
VALUES(@DatabaseName,@BackupPath,@CreatedOn,@CreatedBy)";

                using (var command = new SqlCommand())
                {
                    SqlParameter[] param = new SqlParameter[4];
                    param[0] = new SqlParameter("@DatabaseName", SqlDbType.NVarChar);
                    param[0].Direction = ParameterDirection.Input;
                    param[0].Value = databaseName;
                    command.Parameters.Add(param[0]);
                    param[1] = new SqlParameter("@BackupPath", SqlDbType.NVarChar);
                    param[1].Direction = ParameterDirection.Input;
                    param[1].Value = filePath;
                    command.Parameters.Add(param[1]);
                    param[2] = new SqlParameter("@CreatedOn", SqlDbType.DateTime);
                    param[2].Direction = ParameterDirection.Input;
                    param[2].Value = DateTime.Now;
                    command.Parameters.Add(param[2]);
                    param[3] = new SqlParameter("@CreatedBy", SqlDbType.Int);
                    param[3].Direction = ParameterDirection.Input;
                    param[3].Value = 1;
                    command.Parameters.Add(param[3]);

                    command.Connection = connection;
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 2000;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<string> GetAllUserDatabases()
        {
            var databases = new List<string>();

            DataTable databasesTable;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                databasesTable = connection.GetSchema("Databases");

                connection.Close();
            }

            foreach (DataRow row in databasesTable.Rows)
            {
                string databaseName = row["database_name"].ToString();

                if (_systemDatabaseNames.Contains(databaseName))
                    continue;

                databases.Add(databaseName);
            }

            return databases;
        }

        private string BuildBackupPathWithFilename(string databaseName)
        {
            string filename = string.Format("{0}-{1}.bak", databaseName, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));

            return Path.Combine(_backupFolderFullPath, filename);
        }

        public List<BackupModel> BackupList()
        {
            List<BackupModel> bckpList = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"Select * from tblDatabaseBackup";

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 2000;
                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new BackupModel()
                        {
                            BackupId = reader["BackupId"] != null ? (int)reader["BackupId"] : 0,
                            DatabaseName = reader["DatabaseName"] != null ? (string)reader["DatabaseName"] : string.Empty,
                            BackupPath = reader["BackupPath"] != null ? (string)reader["BackupPath"] : string.Empty,
                        };
                        bckpList.Add(item);

                    }

                }
            }
            return bckpList;
        }

        public int DelBackUp(int BackupId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"Delete from tblDatabaseBackup Where BackupId=@BackupId";
                using (var command = new SqlCommand())
                {
                    var param = new SqlParameter("@BackupId", SqlDbType.Int);
                    param.Direction = ParameterDirection.Input;
                    param.Value = BackupId;
                    command.Parameters.Add(param);
                    command.Connection = connection;
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 2000;
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }





    }











}