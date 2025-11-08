# Railway.app 免費部署完整指南 🚀

## 為什麼選擇 Railway？
- ✅ **完全免費**：每月 500 小時 + $5 免費額度
- ✅ **不會休眠**：24/7 運行
- ✅ **免費資料庫**：PostgreSQL 免費提供
- ✅ **自動部署**：連接 GitHub 自動部署
- ✅ **簡單易用**：5 分鐘完成部署

---

## 📋 部署前準備

### 1. 建立 GitHub Repository
```bash
# 在專案目錄執行
git init
git add .
git commit -m "Initial commit"
git branch -M main
git remote add origin https://github.com/你的帳號/你的專案名稱.git
git push -u origin main
```

### 2. 確認檔案結構
確保專案包含以下檔案：
- ✅ `DiBackend.csproj`
- ✅ `Program.cs`
- ✅ `railway.json`
- ✅ `nixpacks.toml`
- ✅ `Database/InitDatabase_PostgreSQL.sql`

---

## 🚀 Railway 部署步驟

### 步驟 1：註冊 Railway
1. 前往 https://railway.app/
2. 點擊 **"Login"**
3. 選擇 **"Login with GitHub"**
4. 授權 Railway 存取你的 GitHub

### 步驟 2：建立新專案
1. 登入後，點擊 **"New Project"**
2. 選擇 **"Deploy from GitHub repo"**
3. 選擇你剛才上傳的 Repository
4. Railway 會自動開始建置

### 步驟 3：新增 PostgreSQL 資料庫
1. 在專案頁面，點擊 **"+ New"**
2. 選擇 **"Database"**
3. 選擇 **"Add PostgreSQL"**
4. 等待資料庫建立完成（約 30 秒）

### 步驟 4：連接資料庫到應用程式
1. 點擊 PostgreSQL 服務
2. 切換到 **"Variables"** 標籤
3. 複製 **`DATABASE_URL`** 的值（類似：`postgresql://user:pass@host:port/db`）
4. 回到你的應用程式服務
5. 切換到 **"Variables"** 標籤
6. 點擊 **"+ New Variable"**
7. 新增：
   ```
   名稱: DATABASE_URL
   值: <剛才複製的 DATABASE_URL>
   ```
8. 點擊 **"Add"**

### 步驟 5：設定環境變數
在應用程式的 Variables 中新增：
```
ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://0.0.0.0:$PORT
```

### 步驟 6：初始化資料庫
1. 點擊 PostgreSQL 服務
2. 切換到 **"Data"** 標籤
3. 點擊 **"Query"**
4. 複製 `Database/InitDatabase_PostgreSQL.sql` 的內容
5. 貼上並執行

或使用本地工具（推薦）：
```bash
# 安裝 PostgreSQL 客戶端工具
# 使用 Railway 提供的連線資訊連接
psql <DATABASE_URL>

# 執行初始化腳本
\i Database/InitDatabase_PostgreSQL.sql
```

### 步驟 7：取得公開 URL
1. 回到應用程式服務
2. 切換到 **"Settings"** 標籤
3. 找到 **"Domains"** 區塊
4. 點擊 **"Generate Domain"**
5. Railway 會自動產生一個 URL（例如：`your-app.up.railway.app`）

### 步驟 8：測試 API
在瀏覽器開啟：
```
https://your-app.up.railway.app/api/health
```

應該會看到：
```json
{
  "status": "Healthy",
  "version": "1.0.0",
  "timestamp": "2025-01-01T10:00:00"
}
```

### 步驟 9：更新前端 HTML
編輯 `index.html`：
```javascript
// 更新這一行
const apiBaseUrl = "https://your-app.up.railway.app/api";
```

重新上傳到 GitHub：
```bash
git add index.html
git commit -m "Update API URL"
git push
```

Railway 會自動重新部署！

---

## 🎯 完整測試

### 1. 測試健康檢查
```
GET https://your-app.up.railway.app/api/health
```

### 2. 測試載入人員資料
```
POST https://your-app.up.railway.app/api/LoadPersonnelData
```

### 3. 測試新增人員
使用 Postman 或前端頁面測試新增功能

### 4. 開啟前端頁面
將 `index.html` 部署到：
- GitHub Pages（免費）
- Netlify（免費）
- Vercel（免費）

或直接在本地開啟 `index.html`，只要 API URL 正確即可！

---

## 📊 Railway 免費額度說明

### 免費方案包含：
- ✅ 500 小時執行時間/月
- ✅ $5 免費額度
- ✅ 512MB RAM
- ✅ 1GB 磁碟空間
- ✅ PostgreSQL 資料庫（1GB）

### 如何節省額度：
1. 只在需要時運行（可以暫停服務）
2. 使用 Railway 的休眠功能
3. 優化程式碼減少資源使用

---

## 🔧 常見問題

### Q1: 部署失敗怎麼辦？
**A:** 檢查 Railway 的 **"Deployments"** 標籤查看錯誤訊息。常見問題：
- 缺少 `railway.json` 或 `nixpacks.toml`
- .NET 版本不符
- 連線字串錯誤

### Q2: 資料庫連線失敗？
**A:** 確認：
1. `DATABASE_URL` 環境變數已正確設定
2. PostgreSQL 服務正在運行
3. 已執行初始化腳本

### Q3: API 回應 404？
**A:** 檢查：
1. 路由設定是否正確
2. 應用程式是否成功啟動
3. 查看 Railway 的 Logs

### Q4: 如何查看 Logs？
**A:** 在 Railway 專案中，點擊應用程式服務，切換到 **"Logs"** 標籤

### Q5: 如何更新程式碼？
**A:** 只需 push 到 GitHub，Railway 會自動重新部署：
```bash
git add .
git commit -m "Update code"
git push
```

---

## 🎉 部署完成檢查清單

- [ ] Railway 專案已建立
- [ ] PostgreSQL 資料庫已建立並初始化
- [ ] 環境變數已設定
- [ ] 應用程式成功部署
- [ ] 公開 URL 已產生
- [ ] API 健康檢查通過
- [ ] 載入人員資料功能正常
- [ ] 新增人員功能正常
- [ ] 前端 HTML 已更新 API URL
- [ ] 前端可以正常呼叫 API

---

## 📞 需要幫助？

如果遇到問題：
1. 查看 Railway Logs
2. 檢查環境變數設定
3. 確認資料庫連線
4. 查看本專案的 README.md

---

## 🌟 下一步

部署成功後，你可以：
1. 將前端部署到 GitHub Pages
2. 新增更多 API 功能
3. 優化效能
4. 新增使用者認證

**恭喜你完成部署！** 🎊

