using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USh.DataAccess.Data;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;

namespace USh.DataAccess.Repository
{
	public class DomenRepository: Repository<Domen>, IDomenRepository
	{
		private ApplicationDbContext _context;


		public DomenRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
