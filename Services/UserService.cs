using Microsoft.AspNetCore.Http;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class UserService: IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string DecryptString(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decrypted = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decrypted);
        }

        public string PasswordHashCoder(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] encrypted = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }

        public int GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            int Id = int.Parse(userId);
            return Id;
        }
    }
}
