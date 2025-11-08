# 使用 .NET 6.0 SDK 建置
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# 複製專案檔並還原套件
COPY *.csproj ./
RUN dotnet restore

# 複製所有檔案並建置
COPY . ./
RUN dotnet publish -c Release -o out

# 使用 .NET 6.0 Runtime 執行
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

# 設定環境變數
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# 啟動應用程式
ENTRYPOINT ["dotnet", "DiBackend.dll"]

