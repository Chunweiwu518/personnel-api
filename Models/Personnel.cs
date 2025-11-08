using System;

namespace DiBackend.Models
{
    /// <summary>
    /// 人員資料模型 (Personnel)
    /// </summary>
    public class Personnel
    {
        public int PersonnelId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

