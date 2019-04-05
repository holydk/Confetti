using System;
using System.Linq;
using AutoMapper;
using Confetti.Api.Areas.Public.Factories;
using Confetti.Application.Caching;
using Confetti.Application.Catalog;
using Confetti.Application.Installation;
using Confetti.Application.Mapping;
using Confetti.Domain.Interfaces;
using Confetti.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Confetti.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddJsonFormatters(settings =>
                {
                    settings.Formatting = Newtonsoft.Json.Formatting.None;
                    settings.NullValueHandling = NullValueHandling.Ignore;
                });

            services.AddCors();
            
            var migrationsAssembly = typeof(ConfettiContext).Assembly.GetName().Name;
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DbContext, ConfettiContext>(options =>
                options.UseSqlServer(connectionString,
                x => x.MigrationsAssembly(migrationsAssembly)));

            // dynamically load all entity and query type configurations
            var typeConfigurations = typeof(IOrderedMapperProfile).Assembly.GetTypes().Where(type =>
                (type.GetInterface(typeof(IOrderedMapperProfile).Name) == typeof(IOrderedMapperProfile)) 
                  && (!type.IsAbstract));

            services.AddAutoMapper(c => {
                foreach (var typeConfiguration in typeConfigurations)
                {
                    var configuration = (IOrderedMapperProfile)Activator.CreateInstance(typeConfiguration);
                    configuration.Configure(services);

                    c.AddProfile(typeConfiguration);
                }
            });

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<ICacheManager, PerRequestCacheManager>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICatalogModelFactory, CatalogModelFactory>();
            services.AddScoped<ICommonModelFactory, CommonModelFactory>();
            services.AddScoped<IInstallationService, CodeFirstInstallationService>();

            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
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
                // The default HSTS value is 30 days. You may want to change this for 
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(policy =>
            {
                // policy.WithOrigins(
                //     "http://localhost:4000", 
                //     "http://localhost:4200");
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithExposedHeaders("WWW-Authenticate");
            });

            app.UseMvc();

            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Make sure we have the database
                serviceScope.ServiceProvider
                    .GetService<DbContext>().Database.Migrate();
                serviceScope.ServiceProvider
                    .GetService<IInstallationService>()
                    .InstallDataAsync(updateData: false)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}
