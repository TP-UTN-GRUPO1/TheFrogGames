using Application.Helpers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

            modelBuilder.Entity<Platform>(e =>
            {
                e.HasKey(p => p.Id);
                e.HasIndex(p => p.Name).IsUnique();
            });
            modelBuilder.Entity<Genre>(e =>
            {
                e.HasKey(g => g.Id);
                e.HasIndex(g => g.Name).IsUnique();
            });

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Subtotal)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Platforms)
                .WithMany(p => p.Games)
                .UsingEntity(j => j.ToTable("GamePlatforms"));


            modelBuilder.Entity<Game>()
                .HasMany(g => g.Genres)
                .WithMany(ge => ge.Games)
                .UsingEntity(j => j.ToTable("GameGenres"));

            modelBuilder.Entity<User>().HasData(
               new User { Id = 1, Name = "SysAdmin", LastName = "SysAdmin", BirthDate = new DateOnly(2000, 1, 1), Email = "sysadmin@demo.com", Password = HashHelper.ComputeHash("1234"), RoleId = (int)TypeRole.SysAdmin, IsDeleted = false },
                new User { Id = 2, Name = "Admin", LastName = "Admin", BirthDate = new DateOnly(2000, 1, 1), Email = "admin@demo.com", Password = HashHelper.ComputeHash("1234"), RoleId = (int)TypeRole.Admin, IsDeleted = false },
                new User { Id = 3, Name = "User", LastName = "User", BirthDate = new DateOnly(2000, 1, 1), Email = "user@demo.com", Password = HashHelper.ComputeHash("1234"), RoleId = (int)TypeRole.User, IsDeleted = false }
            );
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = nameof(TypeRole.SysAdmin) },
                new Role { Id = 2, Name = nameof(TypeRole.Admin) },
                new Role { Id = 3, Name = nameof(TypeRole.User) }
            );
        }
    }
}
