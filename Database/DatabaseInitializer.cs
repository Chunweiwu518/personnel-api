using System;
using Dapper;

namespace DiBackend.Database
{
    public class DatabaseInitializer
    {
        public static void Initialize()
        {
            try
            {
                var factory = new DbConnectionFactory();
                using var connection = factory.CreateConnection();
                
                Console.WriteLine("開始初始化資料庫...");
                
                // 建立資料表
                var createTableSql = @"
                    CREATE TABLE IF NOT EXISTS Personnel (
                        PersonnelId SERIAL PRIMARY KEY,
                        Name VARCHAR(50) NOT NULL,
                        IdentityNumber VARCHAR(20) NOT NULL UNIQUE,
                        Department VARCHAR(50) NOT NULL,
                        Email VARCHAR(100) NOT NULL,
                        Phone VARCHAR(20) NOT NULL,
                        CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );
                ";
                
                connection.Execute(createTableSql);
                Console.WriteLine("✓ 資料表建立成功");
                
                // 建立索引
                var createIndexSql = @"
                    CREATE INDEX IF NOT EXISTS idx_personnel_createdat ON Personnel(CreatedAt DESC);
                    CREATE INDEX IF NOT EXISTS idx_personnel_identitynumber ON Personnel(IdentityNumber);
                    CREATE INDEX IF NOT EXISTS idx_personnel_department ON Personnel(Department);
                ";
                
                connection.Execute(createIndexSql);
                Console.WriteLine("✓ 索引建立成功");
                
                // 檢查是否已有資料
                var count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Personnel");
                
                if (count == 0)
                {
                    Console.WriteLine("插入測試資料...");
                    
                    // 插入測試資料
                    var insertDataSql = @"
                        INSERT INTO Personnel (Name, IdentityNumber, Department, Email, Phone, CreatedAt) VALUES
                        ('張小明', 'A123456789', '資訊部', 'zhang.xiaoming@company.com', '0912-345-678', CURRENT_TIMESTAMP),
                        ('李美華', 'B234567890', '人資部', 'li.meihua@company.com', '0923-456-789', CURRENT_TIMESTAMP - INTERVAL '1 hour'),
                        ('王大偉', 'C345678901', '業務部', 'wang.dawei@company.com', '0934-567-890', CURRENT_TIMESTAMP - INTERVAL '2 hours'),
                        ('陳雅婷', 'D456789012', '財務部', 'chen.yating@company.com', '0945-678-901', CURRENT_TIMESTAMP - INTERVAL '3 hours'),
                        ('林志明', 'E567890123', '資訊部', 'lin.zhiming@company.com', '0956-789-012', CURRENT_TIMESTAMP - INTERVAL '4 hours');
                    ";
                    
                    connection.Execute(insertDataSql);
                    Console.WriteLine("✓ 測試資料插入成功（5 筆）");
                }
                else
                {
                    Console.WriteLine($"✓ 資料庫已有 {count} 筆資料，跳過測試資料插入");
                }
                
                Console.WriteLine("資料庫初始化完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"資料庫初始化失敗: {ex.Message}");
                Console.WriteLine($"詳細錯誤: {ex}");
                throw;
            }
        }
    }
}

