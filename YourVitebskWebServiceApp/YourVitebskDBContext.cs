using System;
using Microsoft.EntityFrameworkCore;

namespace YourVitebskWebServiceApp
{
    public class YourVitebskDBContext : DbContext
    {
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserDatum> UserData { get; set; }

        public YourVitebskDBContext(DbContextOptions<YourVitebskDBContext> options) : base(options) { }

       
    }
}
