using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Core.Extensions
{
    /// <summary>
    /// Расширения для подключения Swagger
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Зарегистрировать swagger
        /// </summary>
        public static void AddApiSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "News",
                    Description = "News Web API"
                });

                var xmlPath = string.Format(@"{0}PersonalCopywriterAccount.Api.xml",
                    AppDomain.CurrentDomain.BaseDirectory);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });
        }

        /// <summary>
        /// Зарегистрировать ui swagger
        /// </summary>
        /// <param name="app"></param>
        public static void UseApiSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api/swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "News API v1");
            });
        }
    }
}