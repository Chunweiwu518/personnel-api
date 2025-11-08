# 🚀 Railway 部署步驟（完整版）

## ✅ 為什麼選擇 Railway？
- **完全免費**：每月 500 小時 + $5 額度
- **不需要信用卡**
- **5 分鐘完成部署**
- **24/7 運行，不會休眠**
- **自動從 GitHub 部署**
- **免費 PostgreSQL 資料庫**

---

## 📋 部署前檢查清單

確認你的專案包含以下檔案：
- ✅ `DiBackend.csproj`
- ✅ `Program.cs`
- ✅ `Models/Personnel.cs`
- ✅ `Repositories/PersonnelRepository.cs`
- ✅ `Database/DbConnectionFactory.cs`
- ✅ `Database/InitDatabase_PostgreSQL.sql`
- ✅ `railway.json`
- ✅ `nixpacks.toml`
- ✅ `index.html`

---

## 🎯 完整部署流程

### 第一步：上傳到 GitHub

#### 1.1 在 GitHub 建立新 Repository
1. 前往 https://github.com/
2. 登入你的帳號
3. 點擊右上角 **"+"** > **"New repository"**
4. 設定：
   - Repository name: `personnel-api`（或你喜歡的名稱）
   - Description: `人員管理系統後端 API`
   - 選擇 **Public**（免費方案）
   - **不要**勾選 "Add a README file"
5. 點擊 **"Create repository"**

#### 1.2 上傳程式碼到 GitHub
在專案目錄執行以下命令：

```bash
# 初始化 Git（如果還沒有）
git init

# 新增所有檔案
git add .

# 提交
git commit -m "Initial commit - Personnel Management System"

# 設定主分支名稱
git branch -M main

# 連接到 GitHub（替換成你的 Repository URL）
git remote add origin https://github.com/你的帳號/personnel-api.git

# 推送到 GitHub
git push -u origin main
```

**完成後**，重新整理 GitHub 頁面，應該會看到所有檔案已上傳。

---

### 第二步：部署到 Railway

#### 2.1 註冊 Railway
1. 前往 https://railway.app/
2. 點擊 **"Login"**
3. 選擇 **"Login with GitHub"**
4. 授權 Railway 存取你的 GitHub
5. 完成 Email 驗證（檢查信箱）

#### 2.2 建立新專案
1. 登入 Railway 後，點擊 **"New Project"**
2. 選擇 **"Deploy from GitHub repo"**
3. 如果是第一次使用，需要：
   - 點擊 **"Configure GitHub App"**
   - 選擇要授權的 Repository（選擇 `personnel-api`）
   - 點擊 **"Install & Authorize"**
4. 回到 Railway，選擇 `personnel-api` Repository
5. Railway 會自動開始建置（約 2-3 分鐘）

#### 2.3 新增 PostgreSQL 資料庫
1. 在專案頁面，點擊 **"+ New"**
2. 選擇 **"Database"**
3. 選擇 **"Add PostgreSQL"**
4. 等待資料庫建立完成（約 30 秒）
5. 資料庫會自動出現在專案中

#### 2.4 設定環境變數
1. 點擊你的應用程式服務（顯示為 `personnel-api`）
2. 切換到 **"Variables"** 標籤
3. 點擊 PostgreSQL 服務，複製 **`DATABASE_URL`** 的值
4. 回到應用程式服務的 Variables
5. Railway 應該已自動設定 `DATABASE_URL`，如果沒有：
   - 點擊 **"+ New Variable"**
   - 名稱：`DATABASE_URL`
   - 值：貼上剛才複製的值
6. 新增其他環境變數：
   ```
   ASPNETCORE_ENVIRONMENT = Production
   ASPNETCORE_URLS = http://0.0.0.0:$PORT
   ```

#### 2.5 初始化資料庫
**方法一：使用 Railway 的 Query 功能**
1. 點擊 PostgreSQL 服務
2. 切換到 **"Data"** 標籤
3. 點擊 **"Query"**
4. 開啟 `Database/InitDatabase_PostgreSQL.sql`
5. 複製所有內容並貼到 Query 視窗
6. 點擊 **"Run"** 執行

**方法二：使用本地工具（推薦）**
1. 安裝 PostgreSQL 客戶端或使用線上工具
2. 在 Railway PostgreSQL 服務中找到連線資訊
3. 使用 `psql` 或 pgAdmin 連接
4. 執行 `Database/InitDatabase_PostgreSQL.sql`

#### 2.6 產生公開 URL
1. 回到應用程式服務
2. 切換到 **"Settings"** 標籤
3. 找到 **"Networking"** 或 **"Domains"** 區塊
4. 點擊 **"Generate Domain"**
5. Railway 會產生一個 URL，例如：
   ```
   https://personnel-api-production-xxxx.up.railway.app
   ```
6. **記下這個 URL**，等等要用

#### 2.7 測試 API
在瀏覽器開啟：
```
https://你的URL/api/health
```

應該會看到：
```json
{
  "status": "Healthy",
  "version": "1.0.0",
  "timestamp": "2025-01-08T..."
}
```

測試載入人員資料：
```
https://你的URL/api/LoadPersonnelData
```

應該會看到測試資料！

---

### 第三步：更新前端 HTML

#### 3.1 更新 API URL
編輯 `index.html`，找到這一行：
```javascript
const apiBaseUrl = "http://localhost:5000/api";  // 本地測試
```

改成：
```javascript
const apiBaseUrl = "https://你的Railway URL/api";  // 例如：https://personnel-api-production-xxxx.up.railway.app/api
```

#### 3.2 重新上傳到 GitHub
```bash
git add index.html
git commit -m "Update API URL to Railway"
git push
```

Railway 會自動重新部署（約 1-2 分鐘）

---

### 第四步：部署前端 HTML

你有三個選擇：

#### 選項 A：GitHub Pages（推薦）
1. 在 GitHub Repository 設定中
2. 找到 **"Pages"**
3. Source 選擇 **"main"** 分支
4. 點擊 **"Save"**
5. 等待幾分鐘，會得到一個 URL：
   ```
   https://你的帳號.github.io/personnel-api/
   ```

#### 選項 B：Netlify（更快）
1. 前往 https://www.netlify.com/
2. 拖曳 `index.html` 到頁面
3. 立即得到 URL

#### 選項 C：本地開啟
直接雙擊 `index.html` 在瀏覽器開啟即可！

---

## ✅ 完成檢查

### 測試清單
- [ ] Railway 專案已建立
- [ ] PostgreSQL 資料庫已建立
- [ ] 資料庫已初始化（有測試資料）
- [ ] 環境變數已設定
- [ ] 應用程式成功部署
- [ ] 公開 URL 已產生
- [ ] `/api/health` 回應正常
- [ ] `/api/LoadPersonnelData` 回應正常（有資料）
- [ ] 前端 HTML 已更新 API URL
- [ ] 前端可以載入人員清單
- [ ] 前端可以新增人員

---

## 🎉 交付成果

完成後，你會有：

1. **後端 API（Railway）**
   - URL: `https://你的app.up.railway.app`
   - 健康檢查: `https://你的app.up.railway.app/api/health`
   - 載入資料: `https://你的app.up.railway.app/api/LoadPersonnelData`

2. **前端頁面**
   - GitHub Pages: `https://你的帳號.github.io/personnel-api/`
   - 或本地 `index.html`

3. **原始碼**
   - GitHub: `https://github.com/你的帳號/personnel-api`

---

## 🔧 常見問題

### Q1: Railway 建置失敗？
**檢查**：
- 確認 `railway.json` 和 `nixpacks.toml` 存在
- 查看 Railway 的 **"Deployments"** 標籤的錯誤訊息
- 確認所有檔案都已上傳到 GitHub

### Q2: 資料庫連線失敗？
**檢查**：
- `DATABASE_URL` 環境變數是否正確設定
- PostgreSQL 服務是否正在運行
- 是否已執行初始化腳本

### Q3: API 回應 404？
**檢查**：
- URL 是否正確（包含 `/api/`）
- 應用程式是否成功啟動（查看 Logs）
- 路由設定是否正確

### Q4: 前端無法呼叫 API？
**檢查**：
- `index.html` 中的 `apiBaseUrl` 是否正確
- 瀏覽器 Console 是否有 CORS 錯誤
- API URL 是否包含 `https://`

### Q5: 如何查看 Logs？
在 Railway 專案中：
1. 點擊應用程式服務
2. 切換到 **"Logs"** 標籤
3. 即時查看運行日誌

---

## 📞 需要幫助？

如果遇到問題：
1. 查看 Railway 的 **Logs** 標籤
2. 檢查 **Variables** 是否正確設定
3. 確認資料庫已初始化
4. 測試 `/api/health` 端點

---

## 🎊 恭喜！

完成部署後，你就有一個：
- ✅ 可公開存取的後端 API
- ✅ 完整的前端測試頁面
- ✅ 完整的原始碼（GitHub）
- ✅ 符合題目所有要求

**總耗時：約 15-20 分鐘**

---

## 📝 提交給面試官

提供以下資訊：
1. **後端 API URL**: `https://你的app.up.railway.app`
2. **前端頁面 URL**: `https://你的帳號.github.io/personnel-api/`
3. **GitHub Repository**: `https://github.com/你的帳號/personnel-api`
4. **測試帳號**（如果有）

**祝你面試順利！** 🎉

