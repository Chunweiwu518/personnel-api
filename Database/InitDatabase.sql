-- ============================================
-- 人員主檔資料庫初始化腳本
-- ============================================

-- 建立資料庫（如果不存在）
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PersonnelDB')
BEGIN
    CREATE DATABASE PersonnelDB;
END
GO

USE PersonnelDB;
GO

-- 刪除現有資料表（如果存在）
IF OBJECT_ID('dbo.Personnel', 'U') IS NOT NULL
    DROP TABLE dbo.Personnel;
GO

-- 建立人員主檔資料表
CREATE TABLE Personnel (
    PersonnelId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    IdentityNumber NVARCHAR(20) NOT NULL,
    Department NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_Personnel_IdentityNumber UNIQUE (IdentityNumber)
);
GO

-- 建立索引
CREATE INDEX IX_Personnel_CreatedAt ON Personnel(CreatedAt DESC);
CREATE INDEX IX_Personnel_IdentityNumber ON Personnel(IdentityNumber);
CREATE INDEX IX_Personnel_Department ON Personnel(Department);
GO

-- 插入測試資料
INSERT INTO Personnel (Name, IdentityNumber, Department, Email, Phone, CreatedAt) VALUES
    (N'張小明', 'A123456789', N'資訊部', 'zhang.xiaoming@company.com', '0912-345-678', GETDATE()),
    (N'李美華', 'B234567890', N'人資部', 'li.meihua@company.com', '0923-456-789', DATEADD(HOUR, -1, GETDATE())),
    (N'王大偉', 'C345678901', N'業務部', 'wang.dawei@company.com', '0934-567-890', DATEADD(HOUR, -2, GETDATE())),
    (N'陳雅婷', 'D456789012', N'財務部', 'chen.yating@company.com', '0945-678-901', DATEADD(HOUR, -3, GETDATE())),
    (N'林志明', 'E567890123', N'資訊部', 'lin.zhiming@company.com', '0956-789-012', DATEADD(HOUR, -4, GETDATE()));
GO

-- 驗證資料
SELECT PersonnelId, Name, IdentityNumber, Department, Email, Phone, CreatedAt
FROM Personnel
ORDER BY CreatedAt DESC;
GO

PRINT '人員主檔資料庫初始化完成！';
GO

