using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USh.Models.Models;

namespace USh.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        string PasswordHashCoder(string password);
        public string DecryptString(string encryptedText);
    }
}
