using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IServices
{
    public interface IService
    {

        IUserService User { get; }
        IUrlService Url { get; }
        ISingInService SingIn { get; }

    }
}
