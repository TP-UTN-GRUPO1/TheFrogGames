using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class TheFrogGamesDbContext : DbContext
    {
        public TheFrogGamesDbContext(DbContextOptions<TheFrogGamesDbContext> options)
            : base(options)
        {
        }
        public TheFrogGamesDbContext()
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Game)
                .WithMany()
                .HasForeignKey(oi => oi.GameId);


            modelBuilder.Entity<Game>()
                .HasMany(g => g.Platforms)
                .WithMany(p => p.Games)
                .UsingEntity(j => j.ToTable("GamePlatforms"));


            modelBuilder.Entity<Game>()
                .HasMany(g => g.Genres)
                .WithMany(ge => ge.Games)
                .UsingEntity(j => j.ToTable("GameGenres"));

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "SysAdmin", LastName = "SysAdmin", Date = new DateTime(2000, 1, 1), Email = "sysadmin@demo.com", Password = "1234", RoleId = (int)TypeRole.SysAdmin },
                new User { Id = 2, Name = "Admin", LastName = "Admin", Date = new DateTime(2000, 1, 1), Email = "admin@demo.com", Password = "1234", RoleId = (int)TypeRole.Admin },
                new User { Id = 3, Name = "User", LastName = "User", Date = new DateTime(2000, 1, 1), Email = "user@demo.com", Password = "1234", RoleId = (int)TypeRole.User }

                );
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = nameof(TypeRole.SysAdmin) },
                new Role { Id = 2, Name = nameof(TypeRole.Admin) },
                new Role { Id = 3, Name = nameof(TypeRole.User) }
            );
        }
    }
}
