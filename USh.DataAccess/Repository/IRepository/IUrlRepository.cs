using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USh.Models.Models;

namespace USh.DataAccess.Repository.IRepository
{
    public interface IUrlRepository : IRepository<URL>
    {
        public int GenerateUniqueCode();
        public string CreateShortUrl(int UniqueValue, string domen);
    }
}
