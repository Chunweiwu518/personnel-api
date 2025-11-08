using System;
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
            var rawConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL")  // Railway/Render
                ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? Environment.GetEnvironmentVariable("SQLCONNSTR_DefaultConnection")
                ?? "Server=localhost;Database=PersonnelDB;Trusted_Connection=True;TrustServerCertificate=True;";  // 本地預設值

            // 自動偵測資料庫類型
            _dbType = rawConnectionString.Contains("postgres") || rawConnectionString.Contains("Host=")
                ? DatabaseType.PostgreSQL
                : DatabaseType.SqlServer;

            // PostgreSQL 連線字串可能需要轉換格式
            if (_dbType == DatabaseType.PostgreSQL && rawConnectionString.StartsWith("postgresql://"))
            {
                // Render/Railway 提供的格式: postgresql://user:pass@host:port/db
                // Npgsql 需要的格式: Host=host;Port=port;Database=db;Username=user;Password=pass
                _connectionString = ConvertPostgresUrl(rawConnectionString);
            }
            else
            {
                _connectionString = rawConnectionString;
            }
        }

        private string ConvertPostgresUrl(string url)
        {
            try
            {
                // 移除 postgresql:// 前綴
                var uri = new Uri(url);

                var host = uri.Host;
                var port = uri.Port > 0 ? uri.Port : 5432;
                var database = uri.AbsolutePath.TrimStart('/');
                var username = uri.UserInfo.Split(':')[0];
                var password = uri.UserInfo.Split(':')[1];

                return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true;";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PostgreSQL URL 轉換失敗: {ex.Message}");
                Console.WriteLine($"原始 URL: {url}");
                throw new InvalidOperationException($"無法解析 PostgreSQL 連線字串: {ex.Message}", ex);
            }
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

