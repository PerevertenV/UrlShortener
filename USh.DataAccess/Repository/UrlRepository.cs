using USh.DataAccess.Data;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;
using USh.Utility;

namespace USh.DataAccess.Repository
{
    internal class UrlRepository : Repository<URL>, IUrlRepository
    {
        private ApplicationDbContext _context;
        
        public UrlRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
