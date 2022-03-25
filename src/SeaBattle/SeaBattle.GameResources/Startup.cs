using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:44367"; //token server

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
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
                    policy.WithOrigins("https://localhost:44391") //SPA client
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<GameDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddSingleton<IShipStorageUtility, ShipStorageUtility>(sp =>
            {
                int shipCostCoefficient = ushort.Parse(Configuration.GetValue<string>("GameConfig:ShipCostCoefficient"));
                return new ShipStorageUtility(shipCostCoefficient);
            });

            services.AddSingleton<IGameConfig, GameConfig>(sp =>
            {
                ushort minFieldSizeX = ushort.Parse(Configuration.GetValue<string>("GameConfig:MinFieldSizeX"));
                ushort maxFieldSizeX = ushort.Parse(Configuration.GetValue<string>("GameConfig:MaxFieldSizeX"));
                ushort minFieldSizeY = ushort.Parse(Configuration.GetValue<string>("GameConfig:MinFieldSizeY"));
                ushort maxFieldSizeY = ushort.Parse(Configuration.GetValue<string>("GameConfig:MaxFieldSizeY"));
                byte maxNumberOfPlayers =
                    byte.Parse(Configuration.GetValue<string>("GameConfig:MaxNumberOfPlayers"));
                return new GameConfig(minFieldSizeX, maxFieldSizeX, minFieldSizeY, maxFieldSizeY, maxNumberOfPlayers);
            });

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

            app.UseErrorLogger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("default");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//.RequireAuthorization("ApiScope");

                endpoints.MapControllerRoute(name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");//.RequireAuthorization("ApiScope");
            });
        }
    }
}
