# 🚀 快速開始指南

## 專案已經準備好了！

你的專案已經包含：
- ✅ 完整的後端 API（ASP.NET Core + Dapper）
- ✅ 人員管理資料模型
- ✅ PostgreSQL 支援（免費部署用）
- ✅ MSSQL 支援（本地開發用）
- ✅ 前端測試頁面
- ✅ Railway 部署設定

---

## 📋 三種使用方式

### 方式一：直接部署到 Railway（推薦，完全免費）⭐

**優點**：
- 不需要本地安裝資料庫
- 完全免費
- 5 分鐘完成部署
- 24/7 運行

**步驟**：
1. 上傳到 GitHub
2. 按照 `RAILWAY_DEPLOY_GUIDE.md` 部署
3. 完成！

---

### 方式二：本地開發（需要 SQL Server）

**需求**：
- .NET 6.0 SDK
- SQL Server（或 SQL Server Express）

**步驟**：

1. **安裝 SQL Server**（如果還沒有）
   - 下載 SQL Server Express（免費）
   - 或使用 Docker：
     ```bash
     docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
     ```

2. **初始化資料庫**
   - 開啟 SSMS（SQL Server Management Studio）
   - 執行 `Database/InitDatabase.sql`

3. **更新連線字串**
   - 編輯 `appsettings.json`
   - 更新 `DefaultConnection` 為你的 SQL Server 連線字串

4. **執行專案**
   ```bash
   dotnet run
   ```

5. **測試**
   - 開啟 http://localhost:5000

---

### 方式三：使用 PostgreSQL 本地開發

**需求**：
- .NET 6.0 SDK
- PostgreSQL（或 Docker）

**步驟**：

1. **安裝 PostgreSQL**
   ```bash
   # 使用 Docker（推薦）
   docker run --name postgres -e POSTGRES_PASSWORD=yourpassword -p 5432:5432 -d postgres
   ```

2. **初始化資料庫**
   ```bash
   # 連接到 PostgreSQL
   psql -h localhost -U postgres
   
   # 執行腳本
   \i Database/InitDatabase_PostgreSQL.sql
   ```

3. **更新連線字串**
   - 編輯 `appsettings.json`：
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=postgres;Username=postgres;Password=yourpassword"
     }
   }
   ```

4. **執行專案**
   ```bash
   dotnet run
   ```

---

## 🎯 我的建議

### 如果你想快速完成（3天內交付）：
👉 **直接使用方式一：部署到 Railway**

**原因**：
1. 不需要本地安裝資料庫
2. 5 分鐘完成部署
3. 直接得到公開 URL
4. 完全免費
5. 符合題目要求（雲端部署）

### 如果你想本地開發測試：
👉 **使用方式三：PostgreSQL + Docker**

**原因**：
1. 不需要安裝 SQL Server
2. Docker 一鍵啟動
3. 與 Railway 環境一致
4. 方便測試

---

## 📝 立即開始（推薦流程）

### Step 1: 測試本地環境
```powershell
# 執行測試腳本
.\test-local.ps1
```

### Step 2: 上傳到 GitHub
```bash
# 初始化 Git
git init
git add .
git commit -m "Initial commit - Personnel Management System"

# 建立 GitHub Repository（在 GitHub 網站上建立）
# 然後執行：
git remote add origin https://github.com/你的帳號/personnel-api.git
git branch -M main
git push -u origin main
```

### Step 3: 部署到 Railway
按照 `RAILWAY_DEPLOY_GUIDE.md` 的步驟操作

### Step 4: 更新前端
```javascript
// 編輯 index.html，更新 API URL
const apiBaseUrl = "https://your-app.up.railway.app/api";
```

### Step 5: 測試
開啟 `index.html` 測試所有功能

---

## 📂 專案結構說明

```
Di/
├── Models/                          # 資料模型
│   ├── Personnel.cs                # 人員資料模型
│   └── ApiResponse.cs              # API 回應格式
│
├── Repositories/                    # 資料存取層
│   └── PersonnelRepository.cs      # 人員資料存取
│
├── Database/                        # 資料庫腳本
│   ├── DbConnectionFactory.cs      # 資料庫連線工廠
│   ├── InitDatabase.sql            # MSSQL 初始化腳本
│   └── InitDatabase_PostgreSQL.sql # PostgreSQL 初始化腳本
│
├── Program.cs                       # 應用程式進入點
├── DiBackend.csproj                # 專案檔
├── appsettings.json                # 設定檔
│
├── index.html                       # 前端測試頁面
│
├── railway.json                     # Railway 部署設定
├── nixpacks.toml                   # Railway 建置設定
│
├── README.md                        # 專案說明
├── QUICK_START.md                  # 本檔案
├── RAILWAY_DEPLOY_GUIDE.md         # Railway 部署指南
├── DEPLOYMENT_FREE.md              # 免費部署方案比較
│
└── test-local.ps1                  # 本地測試腳本
```

---

## 🔍 API 端點

### 1. 健康檢查
```
GET /api/health
```

### 2. 載入人員資料
```
POST /api/LoadPersonnelData
回應: { "persons": [...] }
```

### 3. 新增人員資料
```
POST /api/AddPersonnelData
參數: Name, IdentityNumber, Department, Email, Phone
回應: { "state": "ok", "data": {...} }
```

---

## ❓ 常見問題

### Q: 我應該選擇哪種方式？
**A:** 如果要快速完成並符合題目要求，選擇**方式一：Railway 部署**

### Q: Railway 真的免費嗎？
**A:** 是的！每月 500 小時 + $5 額度，足夠這個專案使用

### Q: 需要信用卡嗎？
**A:** Railway 不需要信用卡即可使用免費方案

### Q: 如果超過免費額度怎麼辦？
**A:** Railway 會暫停服務，不會收費。這個小專案不會超過額度

### Q: 部署後如何更新程式碼？
**A:** 只需 push 到 GitHub，Railway 會自動重新部署

---

## 📞 需要幫助？

1. 查看 `RAILWAY_DEPLOY_GUIDE.md` 詳細步驟
2. 查看 `README.md` 技術文件
3. 執行 `.\test-local.ps1` 檢查環境

---

## ✅ 檢查清單

部署前：
- [ ] 執行 `.\test-local.ps1` 確認環境
- [ ] 上傳到 GitHub
- [ ] 確認所有檔案都已提交

部署中：
- [ ] 在 Railway 建立專案
- [ ] 新增 PostgreSQL 資料庫
- [ ] 設定環境變數
- [ ] 執行資料庫初始化腳本

部署後：
- [ ] 測試 API 健康檢查
- [ ] 測試載入人員資料
- [ ] 測試新增人員
- [ ] 更新前端 API URL
- [ ] 完整功能測試

---

**準備好了嗎？開始部署吧！** 🚀

建議從 `RAILWAY_DEPLOY_GUIDE.md` 開始！

