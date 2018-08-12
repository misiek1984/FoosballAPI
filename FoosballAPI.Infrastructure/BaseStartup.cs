using System.Collections.Generic;
using FoosballAPI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Reflection;
using FoosballAPI.Infrastructure;
using FoosballAPI.Infrastructure.Configuration;
using FoosballAPI.Infrastructure.Logging;
using Serilog;

namespace FoosballAPI
{
    public abstract class BaseStartup
    {
        protected readonly Container _container = new Container();
        protected readonly IConfiguration _configuration;
        protected readonly IHostingEnvironment _hostingEnvironment;

        protected BaseStartup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _hostingEnvironment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureSimpleInjector(_container)
                .AddCorsMyPolicy()
                .AddMvc(_container)
                .AddSwagger();
        }


        public void Configure(IApplicationBuilder app)
        {
            app
                .UseCustomExceptionHandler(_hostingEnvironment)
                .UseCors(Constants.MyPolicy)
                .UseCustomizedSwagger()
                .UseMvc();

            InternalInitializeContainer(app);

            MigrateDb();
        }

        private void InternalInitializeContainer(IApplicationBuilder app)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            _container.Options.SuppressLifestyleMismatchVerification = false;

            _container.RegisterMvcControllers(app);

            var assemblies = new List<Assembly> { typeof(BaseStartup).Assembly };

            assemblies.AddRange(GetAssemblies());

            _container.Register(typeof(IQueryHandler<,>), assemblies, Lifestyle.Scoped);
            _container.Register(typeof(ICommandHandler<>), assemblies, Lifestyle.Scoped);
         
            _container.Register(() => Log.Logger, Lifestyle.Singleton);
            _container.RegisterDecorator(typeof(ICommandHandler<>), typeof(CommandHandlerLogger<>));
            _container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(QueryHandlerLogger<,>));

            InitializeContainer();

            _container.Verify(VerificationOption.VerifyOnly);
        }

        protected virtual Assembly[] GetAssemblies()
        {
            return new Assembly[] { };
        }

        protected virtual void InitializeContainer()
        {
        }

        protected virtual void MigrateDb()
        {
        }
    }

}

