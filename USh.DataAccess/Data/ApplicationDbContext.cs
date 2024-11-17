using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USh.Models.Models;
using USh.Utility;

namespace USh.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<URL> URL { get; set; }
        public DbSet<Domen> Domens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                
                new User { ID = 1, Login = "admin", role = StaticData.Role_Admin, Password = "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAXSviaP+MhkWco44Ognl56AAAAAACAAAAAAAQZgAAAAEAACAAAACyJUHBMGlxYBZfQLUph+MG7HAoGQ6KyvzExw3ymB9kdAAAAAAOgAAAAAIAACAAAADq+Qk6o9xCSGssQRqt1R7laWl5hfsbtySjX7t1VKpwAhAAAAAD/iqqmULivfii2YSOIstNQAAAAHKfj4xisTwSw1rEF0GSRBIHgHLlJEDU0vO4XIzWCB2PKOxxf97GpgjntB80KbRVjflFHhzugrpfTUV4ilBCvJY=" }
            );
        }
    }
}
