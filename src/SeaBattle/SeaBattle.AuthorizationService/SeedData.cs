// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SeaBattle.AuthorizationService.Data;
using SeaBattle.AuthorizationService.Models;
using Serilog;

namespace SeaBattle.AuthorizationService
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

                    var adminRole = roleMgr.FindByNameAsync("admin").Result;

                    if (adminRole == null)
                    {
                        adminRole = new ApplicationRole
                        {
                            Name = "admin"
                        };

                        var result = roleMgr.CreateAsync(adminRole).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("admin role created");
                    }
                    else
                    {
                        Log.Debug("admin role already exists");
                    }

                    var userRole = roleMgr.FindByNameAsync("user").Result;

                    if (userRole == null)
                    {
                        userRole = new ApplicationRole
                        {
                            Name = "user"
                        };

                        var result = roleMgr.CreateAsync(userRole).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user role created");
                    }
                    else
                    {
                        Log.Debug("user role already exists");
                    }

                    var admin = userMgr.FindByNameAsync("admin").Result;
                    if (admin == null)
                    {
                        admin = new ApplicationUser
                        {
                            UserName = "admin",
                            Email = "Admin@email.com",
                            EmailConfirmed = true,
                            
                        };

                        var result = userMgr.CreateAsync(admin, "Pass$123").Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(admin, new Claim[]{
                            new Claim(JwtClaimTypes.NickName, "admin")
                        }).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("\"admin\" user created");
                    }
                    else
                    {
                        Log.Debug("\"admin\" user already exists");
                    }

                    var user = userMgr.FindByNameAsync("user").Result;

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = "user",
                            Email = "user@email.com",
                            EmailConfirmed = true
                        };

                        var result = userMgr.CreateAsync(user, "Pass$123").Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.NickName, "user")
                        }).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("\"user\" user created");
                    }
                    else
                    {
                        Log.Debug("\"user\" user already exists");
                    }

                    if (!userMgr.IsInRoleAsync(admin, adminRole.Name).Result)
                    {
                        var result = userMgr.AddToRoleAsync(admin, adminRole.Name).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("\"admin\" user added to \"admin\" role");
                    }
                    else
                    {
                        Log.Debug("\"admin\" user already in \"admin\" role");
                    }

                    if (!userMgr.IsInRoleAsync(user, userRole.Name).Result)
                    {
                        var result = userMgr.AddToRoleAsync(user, userRole.Name).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("\"user\" user added to \"user\" role");
                    }
                    else
                    {
                        Log.Debug("\"user\" user already in \"user\" role");
                    }
                }
            }
        }
    }
}
