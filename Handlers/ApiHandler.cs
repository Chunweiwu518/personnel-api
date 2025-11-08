using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;
using DiBackend.Models;
using DiBackend.Repositories;

namespace DiBackend.Handlers
{
    /// <summary>
    /// RESTful API 處理器
    /// </summary>
    public class ApiHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            // 設定 CORS 標頭
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
            context.Response.ContentType = "application/json";

            // 處理 OPTIONS 預檢請求
            if (context.Request.HttpMethod == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                context.Response.End();
                return;
            }

            try
            {
                var path = context.Request.Path.ToLower();
                var method = context.Request.HttpMethod;

                // 路由處理
                if (path.Contains("/loadpersonneldata"))
                {
                    HandleLoadPersonnelData(context);
                }
                else if (path.Contains("/addpersonneldata"))
                {
                    HandleAddPersonnelData(context);
                }
                else if (path.Contains("/api/health"))
                {
                    HandleHealthCheck(context);
                }
                else
                {
                    SendResponse(context, 404, new { error = "API 端點不存在" });
                }
            }
            catch (Exception ex)
            {
                SendResponse(context, 500, new { error = $"伺服器錯誤: {ex.Message}" });
            }
        }

        /// <summary>
        /// 載入人員資料 API
        /// </summary>
        private void HandleLoadPersonnelData(HttpContext context)
        {
            try
            {
                var repository = new PersonnelRepository();
                var personnelList = repository.GetAllPersonnel();

                // 回應格式符合前端需求: { persons: [...] }
                SendResponse(context, 200, new { persons = personnelList });
            }
            catch (Exception ex)
            {
                SendResponse(context, 500, new { error = $"載入資料失敗: {ex.Message}" });
            }
        }

        /// <summary>
        /// 新增人員資料 API
        /// </summary>
        private void HandleAddPersonnelData(HttpContext context)
        {
            try
            {
                // 讀取表單資料
                var name = context.Request.Form["Name"];
                var identityNumber = context.Request.Form["IdentityNumber"];
                var department = context.Request.Form["Department"];
                var email = context.Request.Form["Email"];
                var phone = context.Request.Form["Phone"];

                // 驗證必填欄位
                if (string.IsNullOrEmpty(name) ||
                    string.IsNullOrEmpty(identityNumber) ||
                    string.IsNullOrEmpty(department) ||
                    string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(phone))
                {
                    SendResponse(context, 400, new { state = "error", message = "請填寫所有必填欄位" });
                    return;
                }

                var personnel = new Personnel
                {
                    Name = name,
                    IdentityNumber = identityNumber,
                    Department = department,
                    Email = email,
                    Phone = phone
                };

                var repository = new PersonnelRepository();
                var newId = repository.AddPersonnel(personnel);
                personnel.PersonnelId = newId;

                // 回應格式符合前端需求: { state: "ok" }
                SendResponse(context, 200, new { state = "ok", data = personnel });
            }
            catch (Exception ex)
            {
                SendResponse(context, 500, new { state = "error", message = $"新增失敗: {ex.Message}" });
            }
        }

        /// <summary>
        /// 健康檢查端點
        /// </summary>
        private void HandleHealthCheck(HttpContext context)
        {
            SendResponse(context, 200, new
            {
                status = "Healthy",
                version = "1.0.0",
                timestamp = DateTime.Now
            });
        }

        /// <summary>
        /// 發送 JSON 回應
        /// </summary>
        private void SendResponse(HttpContext context, int statusCode, object data)
        {
            context.Response.StatusCode = statusCode;
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            context.Response.Write(json);
        }
    }
}

