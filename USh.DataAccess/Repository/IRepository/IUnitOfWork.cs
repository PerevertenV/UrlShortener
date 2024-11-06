using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USh.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IUserRepository User {  get; }
        IUrlRepository Url { get; }
        IDomenRepository Domen { get; }
    }
}
