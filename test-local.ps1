# 本地測試腳本
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "本地環境測試腳本" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 1. 檢查 .NET SDK
Write-Host ""
Write-Host "[1/5] 檢查 .NET SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version 2>$null
    if ($dotnetVersion) {
        Write-Host "OK .NET SDK 版本: $dotnetVersion" -ForegroundColor Green
    } else {
        Write-Host "ERROR 未安裝 .NET SDK" -ForegroundColor Red
        Write-Host "下載連結: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
        exit 1
    }
} catch {
    Write-Host "ERROR 未安裝 .NET SDK" -ForegroundColor Red
    exit 1
}

# 2. 還原套件
Write-Host "`n[2/5] 還原 NuGet 套件..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ 套件還原成功" -ForegroundColor Green
} else {
    Write-Host "❌ 套件還原失敗" -ForegroundColor Red
    exit 1
}

# 3. 建置專案
Write-Host "`n[3/5] 建置專案..." -ForegroundColor Yellow
dotnet build --configuration Debug
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ 建置成功" -ForegroundColor Green
} else {
    Write-Host "❌ 建置失敗" -ForegroundColor Red
    exit 1
}

# 4. 檢查檔案
Write-Host "`n[4/5] 檢查必要檔案..." -ForegroundColor Yellow
$requiredFiles = @(
    "Program.cs",
    "DiBackend.csproj",
    "Models/Personnel.cs",
    "Repositories/PersonnelRepository.cs",
    "Database/DbConnectionFactory.cs",
    "index.html"
)

$allFilesExist = $true
foreach ($file in $requiredFiles) {
    if (Test-Path $file) {
        Write-Host "  ✅ $file" -ForegroundColor Green
    } else {
        Write-Host "  ❌ $file (缺少)" -ForegroundColor Red
        $allFilesExist = $false
    }
}

if (-not $allFilesExist) {
    Write-Host "`n❌ 部分檔案缺少，請檢查專案結構" -ForegroundColor Red
    exit 1
}

# 5. 提示下一步
Write-Host "`n[5/5] 測試完成！" -ForegroundColor Yellow
Write-Host "`n========================================" -ForegroundColor Green
Write-Host "✅ 本地環境檢查通過！" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green

Write-Host "`n📝 下一步操作：" -ForegroundColor Cyan
Write-Host "1. 如果要本地測試（需要 SQL Server）：" -ForegroundColor White
Write-Host "   dotnet run" -ForegroundColor Yellow
Write-Host "   然後開啟 http://localhost:5000" -ForegroundColor Yellow

Write-Host "`n2. 如果要部署到 Railway（推薦）：" -ForegroundColor White
Write-Host "   a. 建立 GitHub Repository" -ForegroundColor Yellow
Write-Host "   b. 上傳程式碼到 GitHub" -ForegroundColor Yellow
Write-Host "   c. 按照 RAILWAY_DEPLOY_GUIDE.md 的步驟部署" -ForegroundColor Yellow

Write-Host "`n3. 查看部署指南：" -ForegroundColor White
Write-Host "   cat RAILWAY_DEPLOY_GUIDE.md" -ForegroundColor Yellow

Write-Host "`n========================================`n" -ForegroundColor Cyan

