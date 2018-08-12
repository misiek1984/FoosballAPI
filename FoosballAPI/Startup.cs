using FoosballAPI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Reflection;
using FoosballAPI.Database;
using FoosballAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace FoosballAPI
{
    public class Startup : BaseStartup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
            : base(configuration, env)
        {
        }

        protected override Assembly[] GetAssemblies()
        {
            return new[] { typeof(Startup).Assembly };
        }

        protected override void InitializeContainer()
        {
            _container.Register<IDateTimeProvider, DateTimeProvider>(Lifestyle.Singleton);

            _container.Register(() =>
            {
                var options = new DbContextOptionsBuilder<FoosballDbContext>();
                options.UseSqlServer(_configuration.GetConnectionString(Constants.FoosballDbContext));

                return new FoosballDbContext(options.Options);
            }, Lifestyle.Scoped);
        }

        protected override void MigrateDb()
        {
            using(AsyncScopedLifestyle.BeginScope(_container))
            {
                _container.GetInstance<FoosballDbContext>().Database.MigrateAsync().GetAwaiter().GetResult();
            }
        }
    }

}

