using System;
using System.Net.Http;
using System.Reflection;
using System.Security.Authentication;
using IdentityModel.AspNetCore.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using SeaBattle.GameResources.Utilites;
using SeaBattle.Lib.Data.Entities;
using SeaBattle.Lib.Entities;
using SeaBattle.Lib.Infrastructure;
using SeaBattle.Lib.Managers;
using SeaBattle.Lib.Repositories;

namespace SeaBattle.GameResources
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //IdentityModelEventSource.ShowPII = true;

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["TOKEN_SERVER_DOMAIN"]; //token server

                    //options.Audience = "resourseApi";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };

                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

                    // if token does not contain a dot, it is a reference token
                    options.ForwardDefaultSelector = Selector.ForwardReferenceToken("introspection");
                })
                .AddOAuth2Introspection("introspection", options =>
                {
                    options.Authority = Configuration["TOKEN_SERVER_DOMAIN"]; //token server

                    options.ClientId = "resourseApi";

                    options.ClientSecret = "secret-resourse-api";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "resourse-api");//name api in token server config
                });
            });

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins(Configuration["SPA_CLIENT_DOMAIN"]) //SPA client
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            string connectionString = Config.GetConnectionString(Configuration);

            services.AddDbContext<GameDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddSingleton<IShipStorageUtility, ShipStorageUtility>(sp => Config.GetShipStorageUtility(Configuration));

            services.AddSingleton<IGameConfig, GameConfig>(sp => Config.GetGameConfig(Configuration));

            services.AddScoped<DbContext, GameDbContext>();

            services.AddScoped<GenericRepository<Weapon>>();
            services.AddScoped<GenericRepository<Repair>>();
            services.AddScoped<GenericRepository<Ship>>();
            services.AddScoped<GenericRepository<Game>>();
            services.AddScoped<GenericRepository<GameField>>();
            services.AddScoped<GenericRepository<GameFieldCell>>();
            services.AddScoped<GenericRepository<GamePlayer>>();
            services.AddScoped<GenericRepository<GameShip>>();
            services.AddScoped<GenericRepository<StartField>>();
            services.AddScoped<GenericRepository<StartFieldCell>>();

            services.AddScoped<IShipManager, ShipManager>();
            services.AddScoped<IInitializeManager, InitializeManager>();
            services.AddScoped<IGameFieldActionUtility, GameFieldActionUtility>();
            services.AddScoped<IActionManager, ActionManager>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (Configuration["DBServer"] is not null)
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    using (var context = serviceScope.ServiceProvider.GetService<GameDbContext>())
                    {
                        context.Database.Migrate();
                    }
                }
            }

            app.UseErrorLogger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("default");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization("ApiScope");

                endpoints.MapControllerRoute(name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization("ApiScope");
            });
        }
    }
}
