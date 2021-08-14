using System;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using Core.ConfigureOptions;
using Core.Extensions;
using Core.MapperConfigurations;
using Core.Services.Auth;
using Core.Validators;
using Database;
using Database.Entities.Identity;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("Default")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddService(ConfigureAuthentication);
            services.AddService(ConfigureScopedServices);
            services.AddService(ConfigureOptions);

            services.AddControllers();
            services.AddApiSwagger();
            AddMapster(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseApiSwagger();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void AddMapster(IServiceCollection services)
        {
            var config = new TypeAdapterConfig();
            MapperConfiguration.Configure(config);
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
        }

        private void ConfigureScopedServices(IServiceCollection services)
        {
            services.AddScoped<IdentityValidator>();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var validationOptions = Configuration.GetSection("TokenValidationOptions").Get<TokenValidationOptions>();

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = validationOptions.ValidateIssuer,
                        ValidateAudience = validationOptions.ValidateAudience,
                        ValidateLifetime = validationOptions.ValidateLifetime,
                        ValidateIssuerSigningKey = validationOptions.ValidateIssuerSigningKey,

                        ValidIssuer = validationOptions.ValidIssuer,
                        ValidAudience = validationOptions.ValidAudience,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(validationOptions.IssuerSigningKey))
                    };
                });
            services.Configure<IdentityOptions>(options => { options.Lockout.AllowedForNewUsers = false; });
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<AuthTokenOptions>(Configuration.GetSection("AuthTokenOptions"));
        }

        /// <summary>
        /// Autofac modules registration
        /// </summary>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes().Where(y => y.GetInheritanceChain().Contains(typeof(Module)))).ToList()
                .ForEach(x =>
                {
                    if (Activator.CreateInstance(x) is IModule module) builder.RegisterModule(module);
                });
        }
    }
}