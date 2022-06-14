using Microsoft.EntityFrameworkCore;
using YourVitebskWebServiceApp.Models;

namespace YourVitebskWebServiceApp
{
    public class YourVitebskDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Cafe> Cafes { get; set; }
        public DbSet<CafeType> CafeTypes { get; set; }
        public DbSet<Poster> Posters { get; set; }
        public DbSet<PosterType> PosterTypes { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public YourVitebskDBContext(DbContextOptions<YourVitebskDBContext> options) : base(options) { }

    }
}
