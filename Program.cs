using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DiBackend.Database;

var builder = WebApplication.CreateBuilder(args);

// 加入服務
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 初始化資料庫
try
{
    Console.WriteLine("正在初始化資料庫...");
    DatabaseInitializer.Initialize();
    Console.WriteLine("資料庫初始化成功！");
}
catch (Exception ex)
{
    Console.WriteLine($"資料庫初始化失敗: {ex.Message}");
    // 繼續執行，讓應用程式啟動（可以查看錯誤訊息）
}

// 設定 HTTP 請求管線
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors();
app.UseStaticFiles();

// 設定路由
app.MapGet("/", () => Results.Redirect("/index.html"));

app.MapPost("/api/LoadPersonnelData", () =>
{
    try
    {
        var repository = new DiBackend.Repositories.PersonnelRepository();
        var personnelList = repository.GetAllPersonnel();
        return Results.Json(new { persons = personnelList });
    }
    catch (Exception ex)
    {
        return Results.Json(new { error = $"載入資料失敗: {ex.Message}" }, statusCode: 500);
    }
});

app.MapPost("/api/AddPersonnelData", async (HttpContext context) =>
{
    try
    {
        var form = await context.Request.ReadFormAsync();
        var name = form["Name"].ToString();
        var identityNumber = form["IdentityNumber"].ToString();
        var department = form["Department"].ToString();
        var email = form["Email"].ToString();
        var phone = form["Phone"].ToString();

        if (string.IsNullOrEmpty(name) ||
            string.IsNullOrEmpty(identityNumber) ||
            string.IsNullOrEmpty(department) ||
            string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(phone))
        {
            return Results.Json(new { state = "error", message = "請填寫所有必填欄位" }, statusCode: 400);
        }

        var personnel = new DiBackend.Models.Personnel
        {
            Name = name,
            IdentityNumber = identityNumber,
            Department = department,
            Email = email,
            Phone = phone
        };

        var repository = new DiBackend.Repositories.PersonnelRepository();
        var newId = repository.AddPersonnel(personnel);
        personnel.PersonnelId = newId;

        return Results.Json(new { state = "ok", data = personnel });
    }
    catch (Exception ex)
    {
        return Results.Json(new { state = "error", message = $"新增失敗: {ex.Message}" }, statusCode: 500);
    }
});

app.MapGet("/api/health", () =>
{
    return Results.Json(new
    {
        status = "Healthy",
        version = "1.0.0",
        timestamp = DateTime.Now
    });
});

app.Run();

