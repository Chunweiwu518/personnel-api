using System;

namespace DiBackend.Models
{
    /// <summary>
    /// API 回應統一格式
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}

