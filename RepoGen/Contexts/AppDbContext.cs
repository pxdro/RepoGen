using Microsoft.EntityFrameworkCore;
using RepoGen.Entities;

namespace RepoGen.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<LogContent> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<LogContent>().HasKey(l => new { l.Data, l.User, l.Report });

            modelBuilder.Entity<User>()
                .Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(20);
            modelBuilder.Entity<User>()
                .Property(c => c.Admin)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<Permission>()
                .HasIndex(u => u.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
