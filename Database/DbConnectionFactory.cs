using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace DiBackend.Database
{
    /// <summary>
    /// 資料庫連線工廠（支援 MSSQL 和 PostgreSQL）
    /// </summary>
    public class DbConnectionFactory
    {
        private readonly string _connectionString;
        private readonly DatabaseType _dbType;

        public enum DatabaseType
        {
            SqlServer,
            PostgreSQL
        }

        public DbConnectionFactory()
        {
            // 優先使用環境變數（用於雲端部署）
            _connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")  // Railway/Render
                ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? Environment.GetEnvironmentVariable("SQLCONNSTR_DefaultConnection")
                ?? ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? throw new InvalidOperationException("找不到資料庫連線字串");

            // 自動偵測資料庫類型
            _dbType = _connectionString.Contains("postgres") || _connectionString.Contains("Host=")
                ? DatabaseType.PostgreSQL
                : DatabaseType.SqlServer;
        }

        public DbConnectionFactory(string connectionString, DatabaseType dbType = DatabaseType.SqlServer)
        {
            _connectionString = connectionString;
            _dbType = dbType;
        }

        /// <summary>
        /// 建立資料庫連線
        /// </summary>
        public IDbConnection CreateConnection()
        {
            IDbConnection connection = _dbType == DatabaseType.PostgreSQL
                ? new NpgsqlConnection(_connectionString)
                : new SqlConnection(_connectionString);

            connection.Open();
            return connection;
        }

        /// <summary>
        /// 取得資料庫類型
        /// </summary>
        public DatabaseType GetDatabaseType() => _dbType;

        /// <summary>
        /// 測試資料庫連線
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    return connection.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

