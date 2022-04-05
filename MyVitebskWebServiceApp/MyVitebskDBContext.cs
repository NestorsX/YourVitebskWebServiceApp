using Microsoft.EntityFrameworkCore;
using MyVitebskWebServiceApp.Models;

namespace MyVitebskWebServiceApp
{
    public class MyVitebskDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public MyVitebskDBContext(DbContextOptions<MyVitebskDBContext> options) : base(options) { }
    }
}
