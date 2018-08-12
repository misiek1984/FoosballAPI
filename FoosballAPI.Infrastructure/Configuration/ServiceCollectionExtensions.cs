using FoosballAPI.Infrastructure.Validation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;

namespace FoosballAPI.Infrastructure.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSimpleInjector(this IServiceCollection services, Container container)
        {
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.UseSimpleInjectorAspNetRequestScoping(container);

            return services;
        }

        public static IServiceCollection AddMvc(this IServiceCollection services, Container container)
        {
            services
                .AddMvc(x =>
                {
                    x.Filters.Add(new ValidationFilter());
                })
                .AddJsonOptions(options =>
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            return services;
        }

        public static IServiceCollection AddCorsMyPolicy(this IServiceCollection services)
        {
            return services.AddCors(o => o.AddPolicy(Constants.MyPolicy, builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "FoosballAPI", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
            });
        }
    }

}

