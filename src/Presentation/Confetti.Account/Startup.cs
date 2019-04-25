using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confetti.Account.Infrastructure.Identity.Data;
using Confetti.Account.Infrastructure.Identity.Models;
using Confetti.Account.Infrastructure.Installation;
using Confetti.Account.Infrastructure.Security;
using Confetti.Common.Data;
using Confetti.Common.Installation;
using Confetti.Common.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Confetti.Account
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var dataProviderName = Configuration.GetSection("DataProvider").Value;
            if (dataProviderName == "SQLServer")
            {
                var identityMigrationsAssembly = typeof(AppIdentityDbContext).Assembly.GetName().Name;
                var connectionString = Configuration.GetConnectionString("DefaultConnection");

                // dotnet ef migrations add InitialIdentityDbMigration -c AppIdentityDbContext -o Data/Migrations
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlServer(connectionString,
                        x => x.MigrationsAssembly(identityMigrationsAssembly)));

                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

                services.AddTransient<IDataProvider, SqlServerDataProvider>();
            }
            else
            {
                throw new Exception($"Not supported data provider name: {dataProviderName}");
            }

            services.AddScoped<IPermissionProvider, PermissionProvider>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IInstallationService, CodeFirstInstallationService>();

            // Configure Identity options and password complexity here
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                
                options.Password.RequireNonAlphanumeric = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMvc();

            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Make sure we have the database
                serviceScope.ServiceProvider
                    .GetService<IDataProvider>()
                    .InitializeDatabase();
                // Install sample data
                serviceScope.ServiceProvider
                    .GetService<IInstallationService>()
                    .InstallDataAsync(updateData: true)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
