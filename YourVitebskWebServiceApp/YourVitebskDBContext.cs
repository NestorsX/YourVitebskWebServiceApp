using Microsoft.EntityFrameworkCore;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp
{
    public class YourVitebskDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserDatum> UserData { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public YourVitebskDBContext(DbContextOptions<YourVitebskDBContext> options) : base(options) { }

    }
}
