using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebSockets.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Constants = FoosballAPI.Infrastructure.Constants;

namespace FoosballAPI.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env)
        {
            return app.UseExceptionHandler(
                options => options.Run(async context => await Handle(context, env)));
        }
        private static async Task Handle(HttpContext context, IHostingEnvironment env)
        {
            var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (ex == null)
                return;

            var result = JsonConvert.SerializeObject(
                new
                {
                    ErrorName = ex.GetType().Name,
                    ErrorMessage = ex.Message,
                    ErrorDetails = env.IsEnvironment(Constants.Development) ? ex.StackTrace : null
                },
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            await context.Response.WriteAsync(result);
        }

        public static IApplicationBuilder UseCustomizedSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoosballAPI");
            });
            return app;
        }
    }
}

