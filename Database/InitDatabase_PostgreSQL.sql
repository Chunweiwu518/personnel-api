-- ============================================
-- 人員主檔資料庫初始化腳本 (PostgreSQL)
-- ============================================

-- 刪除現有資料表（如果存在）
DROP TABLE IF EXISTS Personnel;

-- 建立人員主檔資料表
CREATE TABLE Personnel (
    PersonnelId SERIAL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    IdentityNumber VARCHAR(20) NOT NULL UNIQUE,
    Department VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- 建立索引
CREATE INDEX idx_personnel_createdat ON Personnel(CreatedAt DESC);
CREATE INDEX idx_personnel_identitynumber ON Personnel(IdentityNumber);
CREATE INDEX idx_personnel_department ON Personnel(Department);

-- 插入測試資料
INSERT INTO Personnel (Name, IdentityNumber, Department, Email, Phone, CreatedAt) VALUES
    ('張小明', 'A123456789', '資訊部', 'zhang.xiaoming@company.com', '0912-345-678', CURRENT_TIMESTAMP),
    ('李美華', 'B234567890', '人資部', 'li.meihua@company.com', '0923-456-789', CURRENT_TIMESTAMP - INTERVAL '1 hour'),
    ('王大偉', 'C345678901', '業務部', 'wang.dawei@company.com', '0934-567-890', CURRENT_TIMESTAMP - INTERVAL '2 hours'),
    ('陳雅婷', 'D456789012', '財務部', 'chen.yating@company.com', '0945-678-901', CURRENT_TIMESTAMP - INTERVAL '3 hours'),
    ('林志明', 'E567890123', '資訊部', 'lin.zhiming@company.com', '0956-789-012', CURRENT_TIMESTAMP - INTERVAL '4 hours');

-- 驗證資料
SELECT PersonnelId, Name, IdentityNumber, Department, Email, Phone, CreatedAt 
FROM Personnel 
ORDER BY CreatedAt DESC;

-- 顯示完成訊息
SELECT '人員主檔資料庫初始化完成！' AS message;

