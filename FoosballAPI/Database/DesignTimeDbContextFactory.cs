using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FoosballAPI.Database
{
    public class DesignTimeDbContextFactory :
        IDesignTimeDbContextFactory<FoosballDbContext>
    {
        public FoosballDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString(Constants.FoosballDbContext);
            new DbContextOptionsBuilder<FoosballDbContext>().UseSqlServer(connectionString);

            return new FoosballDbContext(new DbContextOptionsBuilder<FoosballDbContext>().Options);
        }

    }

}

