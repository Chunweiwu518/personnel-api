# Azure 部署腳本
# 使用方式: .\deploy-azure.ps1 -ResourceGroup "your-rg" -AppName "your-app" -Location "eastasia"

param(
    [Parameter(Mandatory=$true)]
    [string]$ResourceGroup,
    
    [Parameter(Mandatory=$true)]
    [string]$AppName,
    
    [Parameter(Mandatory=$false)]
    [string]$Location = "eastasia",
    
    [Parameter(Mandatory=$false)]
    [string]$SqlServerName = "$AppName-sql",
    
    [Parameter(Mandatory=$false)]
    [string]$SqlAdminUser = "sqladmin",
    
    [Parameter(Mandatory=$true)]
    [string]$SqlAdminPassword
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Azure 部署腳本 - 人員管理系統" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 1. 登入 Azure
Write-Host "`n[1/7] 檢查 Azure 登入狀態..." -ForegroundColor Yellow
$account = az account show 2>$null
if (-not $account) {
    Write-Host "請先登入 Azure..." -ForegroundColor Yellow
    az login
}

# 2. 建立資源群組
Write-Host "`n[2/7] 建立資源群組: $ResourceGroup" -ForegroundColor Yellow
az group create --name $ResourceGroup --location $Location

# 3. 建立 SQL Server
Write-Host "`n[3/7] 建立 SQL Server: $SqlServerName" -ForegroundColor Yellow
az sql server create `
    --name $SqlServerName `
    --resource-group $ResourceGroup `
    --location $Location `
    --admin-user $SqlAdminUser `
    --admin-password $SqlAdminPassword

# 4. 設定防火牆規則（允許 Azure 服務）
Write-Host "`n[4/7] 設定 SQL Server 防火牆規則..." -ForegroundColor Yellow
az sql server firewall-rule create `
    --resource-group $ResourceGroup `
    --server $SqlServerName `
    --name AllowAzureServices `
    --start-ip-address 0.0.0.0 `
    --end-ip-address 0.0.0.0

# 5. 建立資料庫
Write-Host "`n[5/7] 建立資料庫: PersonnelDB" -ForegroundColor Yellow
az sql db create `
    --resource-group $ResourceGroup `
    --server $SqlServerName `
    --name PersonnelDB `
    --service-objective Basic

# 6. 建立 App Service Plan
Write-Host "`n[6/7] 建立 App Service Plan..." -ForegroundColor Yellow
az appservice plan create `
    --name "$AppName-plan" `
    --resource-group $ResourceGroup `
    --location $Location `
    --sku B1 `
    --is-linux

# 7. 建立 Web App
Write-Host "`n[7/7] 建立 Web App: $AppName" -ForegroundColor Yellow
az webapp create `
    --name $AppName `
    --resource-group $ResourceGroup `
    --plan "$AppName-plan" `
    --runtime "DOTNET|6.0"

# 8. 設定連線字串
Write-Host "`n[8/8] 設定連線字串..." -ForegroundColor Yellow
$connectionString = "Server=tcp:$SqlServerName.database.windows.net,1433;Database=PersonnelDB;User ID=$SqlAdminUser;Password=$SqlAdminPassword;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

az webapp config connection-string set `
    --name $AppName `
    --resource-group $ResourceGroup `
    --connection-string-type SQLAzure `
    --settings DefaultConnection=$connectionString

# 9. 發佈應用程式
Write-Host "`n[9/9] 發佈應用程式..." -ForegroundColor Yellow
dotnet publish -c Release -o ./publish

Compress-Archive -Path ./publish/* -DestinationPath deploy.zip -Force

az webapp deployment source config-zip `
    --resource-group $ResourceGroup `
    --name $AppName `
    --src deploy.zip

# 清理
Remove-Item deploy.zip
Remove-Item -Recurse -Force ./publish

Write-Host "`n========================================" -ForegroundColor Green
Write-Host "部署完成！" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host "`nWeb App URL: https://$AppName.azurewebsites.net" -ForegroundColor Cyan
Write-Host "SQL Server: $SqlServerName.database.windows.net" -ForegroundColor Cyan
Write-Host "`n下一步:" -ForegroundColor Yellow
Write-Host "1. 使用 SSMS 連線到 SQL Server 並執行 Database/InitDatabase.sql" -ForegroundColor White
Write-Host "2. 更新 index.html 中的 API URL" -ForegroundColor White
Write-Host "3. 測試應用程式功能" -ForegroundColor White

