# 人員管理系統 - 後端測試專案

## 專案說明
這是一個使用 C# ASP.NET Core 和 Dapper 框架開發的人員管理系統後端 API。

## 技術架構
- **後端框架**: ASP.NET Core 6.0
- **ORM 框架**: Dapper
- **資料庫**: Microsoft SQL Server
- **前端**: HTML + Bootstrap 5 + jQuery

## 專案結構
```
Di/
├── Models/                 # 資料模型
│   ├── Personnel.cs       # 人員資料模型
│   └── ApiResponse.cs     # API 回應格式
├── Repositories/          # 資料存取層
│   └── PersonnelRepository.cs
├── Database/              # 資料庫腳本
│   └── InitDatabase.sql  # 資料庫初始化腳本
├── Handlers/              # API 處理器（Web Form 版本）
│   └── ApiHandler.cs
├── Program.cs             # 應用程式進入點
├── index.html             # 前端測試頁面
└── appsettings.json       # 設定檔
```

## API 端點

### 1. 載入人員資料
- **URL**: `/api/LoadPersonnelData`
- **方法**: POST
- **回應格式**:
```json
{
  "persons": [
    {
      "PersonnelId": 1,
      "Name": "張小明",
      "IdentityNumber": "A123456789",
      "Department": "資訊部",
      "Email": "zhang@company.com",
      "Phone": "0912-345-678",
      "CreatedAt": "2025-01-01T10:00:00"
    }
  ]
}
```

### 2. 新增人員資料
- **URL**: `/api/AddPersonnelData`
- **方法**: POST
- **參數**: 
  - Name (姓名)
  - IdentityNumber (身分證號)
  - Department (部門)
  - Email
  - Phone (電話)
- **回應格式**:
```json
{
  "state": "ok",
  "data": { ... }
}
```

### 3. 健康檢查
- **URL**: `/api/health`
- **方法**: GET

## 本地開發環境設定

### 1. 安裝必要軟體
- .NET 6.0 SDK 或更高版本
- SQL Server 2019 或更高版本（或 SQL Server Express）
- Visual Studio 2022 或 VS Code

### 2. 設定資料庫
1. 開啟 SQL Server Management Studio (SSMS)
2. 執行 `Database/InitDatabase.sql` 腳本
3. 確認資料庫 `PersonnelDB` 已建立並包含測試資料

### 3. 更新連線字串
編輯 `appsettings.json`，更新資料庫連線字串：
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PersonnelDB;User Id=sa;Password=你的密碼;TrustServerCertificate=True;"
  }
}
```

### 4. 安裝套件
```bash
dotnet restore
```

### 5. 執行專案
```bash
dotnet run
```

應用程式將在 `http://localhost:5000` 啟動。

### 6. 測試 API
開啟瀏覽器訪問 `http://localhost:5000` 即可看到測試頁面。

## Azure 部署步驟

### 方法一：使用 Azure Portal

1. **建立 Azure SQL Database**
   - 登入 Azure Portal
   - 建立新的 SQL Database
   - 記錄連線字串

2. **建立 App Service**
   - 建立新的 Web App
   - 選擇 .NET 6 運行環境
   - 設定連線字串（在 Configuration > Connection strings）

3. **部署程式碼**
   ```bash
   # 發佈專案
   dotnet publish -c Release -o ./publish
   
   # 壓縮檔案
   Compress-Archive -Path ./publish/* -DestinationPath deploy.zip
   
   # 使用 Azure CLI 部署
   az webapp deployment source config-zip --resource-group <資源群組> --name <應用程式名稱> --src deploy.zip
   ```

4. **執行資料庫腳本**
   - 使用 SSMS 連線到 Azure SQL Database
   - 執行 `Database/InitDatabase.sql`

5. **更新前端 API 路徑**
   - 編輯 `index.html`
   - 將 `apiBaseUrl` 更新為 Azure URL

### 方法二：使用 Visual Studio

1. 右鍵點擊專案 > 發佈
2. 選擇 Azure > Azure App Service
3. 建立新的 App Service 或選擇現有的
4. 設定連線字串
5. 點擊發佈

### 方法三：使用 GitHub Actions（推薦）

1. 在 Azure Portal 建立 App Service
2. 下載發佈設定檔
3. 在 GitHub Repository 設定 Secret
4. 使用提供的 GitHub Actions workflow

## 環境變數設定（Azure）

在 Azure App Service 的 Configuration 中設定：

```
SQLCONNSTR_DefaultConnection = Server=tcp:your-server.database.windows.net,1433;Database=PersonnelDB;User ID=your-user;Password=your-password;Encrypt=True;TrustServerCertificate=False;
```

## 測試

### 本地測試
1. 啟動應用程式
2. 開啟 `http://localhost:5000`
3. 測試載入人員清單功能
4. 測試新增人員功能

### 雲端測試
1. 部署完成後，訪問 Azure URL
2. 確認 API 正常運作
3. 測試所有功能

## 常見問題

### Q: 無法連線到資料庫
A: 檢查連線字串是否正確，確認 SQL Server 服務已啟動。

### Q: CORS 錯誤
A: 確認 Program.cs 中已正確設定 CORS 政策。

### Q: Azure 部署後無法連線資料庫
A: 檢查 Azure SQL 防火牆規則，確保允許 Azure 服務存取。

## 聯絡資訊
如有問題，請聯繫開發團隊。

