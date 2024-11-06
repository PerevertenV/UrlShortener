using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface ISingInService
    {
        public void SignInUser(string login, string role, int userId);
    }
}
