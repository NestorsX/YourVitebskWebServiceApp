using Microsoft.EntityFrameworkCore;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp
{
    public class YourVitebskDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public YourVitebskDBContext(DbContextOptions<YourVitebskDBContext> options) : base(options) { }
    }
}
