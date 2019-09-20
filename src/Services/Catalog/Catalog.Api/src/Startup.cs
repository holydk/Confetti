using System;
using System.Linq;
using AutoMapper;
using Confetti.Catalog.Application;
using Catalog.Infrastructure.Application;
using Confetti.Catalog.Infrastructure.Data.EntityFrameworkCore;
using Confetti.Data;
using Confetti.Infrastructure.Data.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var dataProviderName = Configuration.GetSection("DataProvider").Value;
            if (dataProviderName == "SQLServer")
            {
                var migrationsAssembly = typeof(CatalogContext).Assembly.GetName().Name;
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<DbContext, CatalogContext>(options =>
                    options
                        .UseSqlServer(connectionString, x => x.MigrationsAssembly(migrationsAssembly))
                        .UseMappingAssembly());
            }
            else
            {
                throw new Exception($"Not supported data provider name: {dataProviderName}");
            }

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddAutoMapper();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            // using (var serviceScope = app.ApplicationServices
            //     .GetRequiredService<IServiceScopeFactory>().CreateScope())
            // {
            //     // Make sure we have the database
            //     var c = serviceScope.ServiceProvider.GetService<ICategoryService>();

            //     var all = new System.Collections.Generic.List<long>();
            //     var count = 1000;
            //     for (int i = 0; i < count; i++)
            //     {
            //         var watch = System.Diagnostics.Stopwatch.StartNew();
            //         var categories = c.GetCategoriesAsync().GetAwaiter().GetResult();
            //         watch.Stop();
            //         var elapsedMs = watch.ElapsedMilliseconds;
            //         all.Add(elapsedMs);
            //         Console.WriteLine(categories.Count);
            //     }
            //     Console.WriteLine($"Avg {all.Sum() / count}");
            // }
        }
    }
}
