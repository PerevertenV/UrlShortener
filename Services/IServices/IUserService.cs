using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IUserService
    {
        public string PasswordHashCoder(string password);
        public string DecryptString(string encryptedText);
        public int GetUserId();
    }
}
