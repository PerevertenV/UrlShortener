using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using USh.DataAccess.Data;
using System.Security.Claims;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;

namespace USh.DataAccess.Repository
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
