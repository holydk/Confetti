using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Confetti.Account.Application;
using Confetti.Account.Application.Configuration;
using Confetti.Account.Application.Models;
using Confetti.Account.Infrastructure;
using Confetti.Account.Infrastructure.Identity;
using Confetti.Account.Infrastructure.Identity.Data;
using Confetti.Account.Infrastructure.Identity.Models;
using Confetti.Account.Infrastructure.Identity.Validators;
using Confetti.Common.Data;
using Confetti.Common.Installation;
using Confetti.Identity.Configuration;
using Confetti.Identity.Factories;
using Confetti.Identity.Infrastructure.Data;
using Confetti.Identity.Infrastructure.Factories;
using Confetti.Identity.Services;
using Confetti.Identity.Validators;
using Confetti.Identity.ViewModels;
using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Confetti.Identity
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCors();

            services.Configure<ConfettiIdentityOptions>(Configuration.GetSection("IdentityOptions"));
            services.Configure<AccountOptions>(Configuration);
            services.AddScoped<DefaultIdentityErrorDescriber>();
            services.AddTransient<IValidator<LoginInputModel>, LoginInputModelValidator>();
            services.AddTransient<IValidator<LoginViewModel>, LoginInputModelValidator>();
            services.AddTransient<IValidator<RegisterInputModel>, RegisterInputModelValidator>();
            services.AddTransient<IValidator<RegisterViewModel>, RegisterInputModelValidator>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv =>
                {
                    // fv.ValidatorFactory = new DependencyResolverModelValidatorFactory(
                    //     services
                    // );
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                });;

            var builder = services.AddConfettiIdentityServer();

            var dataProviderName = Configuration.GetSection("DataProvider").Value;
            if (dataProviderName == "SQLServer")
            {
                var migrationsAssembly = typeof(Startup).Assembly.GetName().Name;
                var identityMigrationsAssembly = typeof(AppIdentityDbContext).Assembly.GetName().Name;
                var connectionString = Configuration.GetConnectionString("DefaultConnection");

                // dotnet ef migrations add InitialIdentityDbMigration -c AppIdentityDbContext -o Infrastructure/Data/Migrations
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlServer(connectionString,
                        x => x.MigrationsAssembly(identityMigrationsAssembly)));

                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders()
                    .AddErrorDescriber<DefaultIdentityErrorDescriber>()
                    .AddUserValidator<DefaultUserValidator<ApplicationUser>>()
                    .AddPasswordValidator<DefaultPasswordValidator<ApplicationUser>>();
                
                // this adds the config data from DB (clients, resources)
                // dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Infrastructure/Data/Migrations/IdentityServer/ConfigurationDb
                builder.AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                // dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<ApplicationUser>();

                builder.AddAnonymousAuthentication();

                if (Environment.IsDevelopment())
                {
                    builder.AddDeveloperSigningCredential();
                }
                else
                {
                    throw new Exception("Need to configure key material.");
                }

                services.AddTransient<IDataProvider, Infrastructure.Data.SqlServerDataProvider>();
            }
            else
            {
                throw new Exception($"Not supported data provider name: {dataProviderName}");
            }

            var protectionBuilder = services.AddDataProtection()
                .SetApplicationName("hrselfservice")
                .PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory()));
            
            if (Environment.IsDevelopment())
            {
                // var certPath = Path.Combine(Directory.GetCurrentDirectory(), "signInMessage.cer");
                // var cert = new X509Certificate2(certPath, "b4820f4a-a763", X509KeyStorageFlags.Exportable);
                
                // var X509_CERTIFICATE_THUMBPRINT = "3f40db58807b6d507976c8ef50a3230daf2a1265";
                // X509Store store = new X509Store(StoreLocation.CurrentUser);
                // store.Open(OpenFlags.ReadOnly);
                // X509Certificate2Collection x509Certificate2Collection = store.Certificates.Find(X509FindType.FindByThumbprint, X509_CERTIFICATE_THUMBPRINT, false);
                // X509Certificate2 x509Cert = x509Certificate2Collection.Count > 0 ? x509Certificate2Collection[0] : null;
                // store.Close();

                // var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                // store.Open(OpenFlags.ReadWrite);
                // store.Add(cert);
                // store.Close();

                // X509Certificate2 certificate = null;

                // // Load certificate from Certificate Store using the configured Thumbprint
                // using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
                // {
                //     store.Open(OpenFlags.ReadOnly);
                //     X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindByThumbprint, X509_CERTIFICATE_THUMBPRINT, false);

                //     if (certificates.Count > 0)
                //         certificate = certificates[0];
                // }

                // if (certificate == null)
                //     throw new Exception($"Certificate {X509_CERTIFICATE_THUMBPRINT} not found.");

                var certPath = Path.Combine(Directory.GetCurrentDirectory(), "cfTest5.pfx");
                var cert = new X509Certificate2(certPath, "test", X509KeyStorageFlags.Exportable);

                var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Add(cert);
                store.Close();

                // protectionBuilder.ProtectKeysWithProvidedCertificate(cert);
                // protectionBuilder.ProtectKeysWithCertificate(cert);
                // protectionBuilder.ProtectKeysWithCertificate("c7cd1747fb05d0f2ce4fdc7166ed05516db00db1");
                // protectionBuilder.ProtectKeysWithCertificate(certificate);
                // protectionBuilder.UnprotectKeysWithAnyCertificate(certificate);
            }

            services.AddScoped<IInstallationService, CodeFirstInstallationService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IIdentityFactory, IdentityFactory>();
            services.AddScoped<ISignInService, SignInService>();

            // Configure Identity options and password complexity here
            services.Configure((Microsoft.AspNetCore.Identity.IdentityOptions options) =>
            {
                options.User.RequireUniqueEmail = true;
                
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddAuthentication();
                // .AddIdentityServerAuthentication(options => {
                //     options.RequireHttpsMetadata = false;
                // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithExposedHeaders("WWW-Authenticate");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseConfettiIdentityServer();

            // app.UseMvc(routes =>
            // {
            //     routes.MapRoute(
            //         name: "default",
            //         template: "{controller=Home}/{action=Index}/{id?}");
            // });
            app.UseMvcWithDefaultRoute();

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
