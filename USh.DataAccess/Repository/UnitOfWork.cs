using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USh.DataAccess.Data;
using USh.DataAccess.Repository.IRepository;

namespace USh.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public IUserRepository User { get; private set; }
        public IUrlRepository Url { get; private set; }
        public IDomenRepository Domen { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            User = new UserRepository(_context);
            Url = new UrlRepository(_context);
            Domen = new DomenRepository(_context);
        }
    }
}
