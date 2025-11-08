using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DiBackend.Database;
using DiBackend.Models;

namespace DiBackend.Repositories
{
    /// <summary>
    /// 人員資料存取層
    /// </summary>
    public class PersonnelRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public PersonnelRepository()
        {
            _dbFactory = new DbConnectionFactory();
        }

        /// <summary>
        /// 取得所有人員資料
        /// </summary>
        public List<Personnel> GetAllPersonnel()
        {
            using (var connection = _dbFactory.CreateConnection())
            {
                var sql = @"
                    SELECT PersonnelId, Name, IdentityNumber, Department, Email, Phone, CreatedAt
                    FROM Personnel
                    ORDER BY CreatedAt DESC";

                return connection.Query<Personnel>(sql).ToList();
            }
        }

        /// <summary>
        /// 根據 ID 取得人員
        /// </summary>
        public Personnel? GetPersonnelById(int id)
        {
            using (var connection = _dbFactory.CreateConnection())
            {
                var sql = @"
                    SELECT PersonnelId, Name, IdentityNumber, Department, Email, Phone, CreatedAt
                    FROM Personnel
                    WHERE PersonnelId = @PersonnelId";

                return connection.QueryFirstOrDefault<Personnel>(sql, new { PersonnelId = id });
            }
        }

        /// <summary>
        /// 新增人員
        /// </summary>
        public int AddPersonnel(Personnel personnel)
        {
            using (var connection = _dbFactory.CreateConnection())
            {
                personnel.CreatedAt = DateTime.Now;

                // 判斷資料庫類型
                var connectionString = connection.ConnectionString;
                string sql;

                if (connectionString.Contains("Host=") || connectionString.Contains("Server=") && connectionString.Contains("5432"))
                {
                    // PostgreSQL
                    sql = @"
                        INSERT INTO Personnel (Name, IdentityNumber, Department, Email, Phone, CreatedAt)
                        VALUES (@Name, @IdentityNumber, @Department, @Email, @Phone, @CreatedAt)
                        RETURNING PersonnelId;";
                }
                else
                {
                    // MSSQL
                    sql = @"
                        INSERT INTO Personnel (Name, IdentityNumber, Department, Email, Phone, CreatedAt)
                        VALUES (@Name, @IdentityNumber, @Department, @Email, @Phone, @CreatedAt);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
                }

                return connection.ExecuteScalar<int>(sql, personnel);
            }
        }

        /// <summary>
        /// 更新人員
        /// </summary>
        public bool UpdatePersonnel(Personnel personnel)
        {
            using (var connection = _dbFactory.CreateConnection())
            {
                var sql = @"
                    UPDATE Personnel
                    SET Name = @Name,
                        IdentityNumber = @IdentityNumber,
                        Department = @Department,
                        Email = @Email,
                        Phone = @Phone
                    WHERE PersonnelId = @PersonnelId";

                var rowsAffected = connection.Execute(sql, personnel);
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// 刪除人員
        /// </summary>
        public bool DeletePersonnel(int id)
        {
            using (var connection = _dbFactory.CreateConnection())
            {
                var sql = "DELETE FROM Personnel WHERE PersonnelId = @PersonnelId";
                var rowsAffected = connection.Execute(sql, new { PersonnelId = id });
                return rowsAffected > 0;
            }
        }
    }
}

