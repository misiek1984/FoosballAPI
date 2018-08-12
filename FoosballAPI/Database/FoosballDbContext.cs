using FoosballAPI.Database.Entities;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FoosballAPI.Database
{
    public class FoosballDbContext : DbContext
    {
        public FoosballDbContext(DbContextOptions<FoosballDbContext> options) : base(options)
        {
        }

        public DbSet<TeamEntity> Teams
        {
            get; set;
        }

        public DbSet<GameEntity> Games
        {
            get; set;
        }

        public DbSet<SetEntity> Sets
        {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<GameEntity>().HasOne(x => x.FirstTeam);
            modelBuilder.Entity<GameEntity>().HasOne(x => x.SecondTeam);

            modelBuilder.Entity<SetEntity>().HasOne(x => x.Parent).WithMany(x => x.Sets);
            modelBuilder.Entity<SetEntity>().HasKey(x => new { x.Id, x.ParentId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString(Constants.FoosballDbContext);

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
