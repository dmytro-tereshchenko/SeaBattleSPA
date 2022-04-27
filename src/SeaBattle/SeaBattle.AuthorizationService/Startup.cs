using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeaBattle.AuthorizationService.Data;
using SeaBattle.AuthorizationService.IdentityServer.Helpers;
using SeaBattle.AuthorizationService.Models;
using SeaBattle.AuthorizationService.Services;
using System.Linq;

namespace SeaBattle.AuthorizationService
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddScoped<IAccountControllerHelper, AccountControllerHelper>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<ProfileService>();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            string connectionString;

            if (Configuration["DBServer"] is null)
            {
                //in case of local db server
                connectionString = Configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                //in case of containerization in docker
                string server = Configuration["DBServer"] ?? Configuration.GetValue<string>("ConnectionDb:Server");
                string port = Configuration["DBPort"] ?? Configuration.GetValue<string>("ConnectionDb:Port");
                string user = Configuration["DBUser"] ?? Configuration.GetValue<string>("ConnectionDb:User");
                string password = Configuration["DBPassword"] ?? Configuration.GetValue<string>("ConnectionDb:Password");
                string database = Configuration["Database"] ?? Configuration.GetValue<string>("ConnectionDb:Database");
                connectionString = $"Server={server},{port};Initial Catalog={database};User ID={user};Password={password}";
            }
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                    options.EmitStaticAudienceClaim = true;
                })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>();


            // not recommended for production - you need to store your key material somewhere secure
            //builder.AddDeveloperSigningCredential();

            // credentials for prod
            var rsa = new RsaKeyService(Environment, TimeSpan.FromDays(30));
            services.AddSingleton<RsaKeyService>(provider => rsa);
            builder.AddSigningCredential(rsa.GetKey(), IdentityServerConstants.RsaSigningAlgorithm.RS512);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    //Create and initiate db
                    SeedData.EnsureSeedData(context.Database.GetConnectionString());
                }
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
