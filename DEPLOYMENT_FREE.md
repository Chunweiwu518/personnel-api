# 免費部署指南

## 🚀 方案一：Railway.app（最推薦）

### 優點
- ✅ 完全免費（每月 500 小時 + $5 免費額度）
- ✅ 支援 PostgreSQL 資料庫（免費）
- ✅ 自動從 GitHub 部署
- ✅ 提供 HTTPS
- ✅ 不會休眠

### 部署步驟

#### 1. 註冊 Railway
1. 前往 https://railway.app/
2. 使用 GitHub 帳號登入
3. 驗證 Email

#### 2. 建立專案
1. 點擊 "New Project"
2. 選擇 "Deploy from GitHub repo"
3. 選擇你的專案 Repository

#### 3. 新增 PostgreSQL 資料庫
1. 在專案中點擊 "+ New"
2. 選擇 "Database" > "Add PostgreSQL"
3. Railway 會自動建立資料庫並提供連線字串

#### 4. 設定環境變數
在 Railway 專案設定中新增：
```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<Railway 提供的 PostgreSQL 連線字串>
```

#### 5. 部署
- Railway 會自動偵測 .NET 專案並部署
- 等待部署完成（約 2-3 分鐘）
- 取得公開 URL

---

## 🌐 方案二：Render.com

### 優點
- ✅ 完全免費
- ✅ 支援 PostgreSQL
- ✅ 自動 HTTPS
- ⚠️ 閒置 15 分鐘後會休眠

### 部署步驟

#### 1. 註冊 Render
1. 前往 https://render.com/
2. 使用 GitHub 帳號註冊

#### 2. 建立 Web Service
1. 點擊 "New +" > "Web Service"
2. 連接 GitHub Repository
3. 設定：
   - Name: personnel-api
   - Environment: Docker
   - Region: Singapore
   - Branch: main
   - Plan: Free

#### 3. 建立 PostgreSQL 資料庫
1. 點擊 "New +" > "PostgreSQL"
2. 選擇 Free 方案
3. 記錄 Internal Database URL

#### 4. 設定環境變數
```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<Render PostgreSQL URL>
```

---

## ☁️ 方案三：Fly.io

### 優點
- ✅ 免費額度充足
- ✅ 全球 CDN
- ✅ 支援 SQLite（免費）或 PostgreSQL

### 部署步驟

#### 1. 安裝 Fly CLI
```bash
# Windows (PowerShell)
iwr https://fly.io/install.ps1 -useb | iex
```

#### 2. 登入
```bash
fly auth login
```

#### 3. 初始化專案
```bash
fly launch
```

#### 4. 部署
```bash
fly deploy
```

---

## 📊 方案比較

| 功能 | Railway | Render | Fly.io |
|------|---------|--------|--------|
| 免費額度 | 500 小時/月 | 750 小時/月 | 充足 |
| 資料庫 | PostgreSQL | PostgreSQL | SQLite/PostgreSQL |
| 休眠 | ❌ 不休眠 | ✅ 15分鐘後休眠 | ❌ 不休眠 |
| 部署難度 | ⭐ 簡單 | ⭐⭐ 中等 | ⭐⭐⭐ 較難 |
| 推薦度 | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ |

---

## 🎯 我的推薦

**使用 Railway.app**，原因：
1. 完全免費且額度充足
2. 不會休眠（使用體驗最好）
3. 部署最簡單
4. 提供免費 PostgreSQL
5. 自動 HTTPS

---

## 📝 注意事項

### 資料庫選擇
由於免費方案通常不提供 MSSQL，我們需要：
1. **改用 PostgreSQL**（推薦）- Railway 和 Render 都免費提供
2. **改用 SQLite**（簡單但功能較少）

### 需要修改的地方
1. 更新 `appsettings.json` 的連線字串
2. 如果使用 PostgreSQL，需要稍微調整 SQL 語法
3. 更新 `index.html` 的 API URL

---

## 🔧 下一步

請告訴我你想使用哪個方案，我會幫你：
1. 調整程式碼以支援該平台
2. 建立部署設定檔
3. 提供詳細的部署步驟

**建議：選擇 Railway.app** 🚀

