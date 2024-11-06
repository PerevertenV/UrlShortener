using Microsoft.AspNetCore.Http;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USh.DataAccess.Data;

namespace Services
{
    public class Service : IService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IUserService User { get; private set; }
        public IUrlService Url { get; private set; }
        public ISingInService SingIn { get; private set; }
        public Service(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            User = new UserService(_httpContextAccessor);
            Url = new UrlService(_context);
            SingIn = new SingInService(_httpContextAccessor);

        }
    }
}
