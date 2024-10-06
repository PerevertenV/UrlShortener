using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USh.Models.Models;

namespace USh.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> User {  get; set; }
        public DbSet<URL> URL {  get; set; }
        public DbSet<Domen> Domens { get; set; }

    }
}
