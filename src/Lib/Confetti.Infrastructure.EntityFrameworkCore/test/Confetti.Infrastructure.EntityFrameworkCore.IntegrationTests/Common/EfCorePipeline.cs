using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Threading;
using Confetti.Domain.Repositories;
using Confetti.Domain.Uow;
using Confetti.Infrastructure.EntityFrameworkCore.IntegrationTests.Domain;
using Confetti.Infrastructure.EntityFrameworkCore.Repositories;
using Confetti.Infrastructure.EntityFrameworkCore.Uow;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Confetti.Infrastructure.EntityFrameworkCore.IntegrationTests
{
    public class EfCorePipeline : IDisposable
    {
        public IServiceCollection ServiceCollection { get; set; }
        public IServiceProvider ServiceProvider => ServiceCollection?.BuildServiceProvider();
        private DbConnection _connection;
        private IServiceScope _scope;

        public void Initialize(UnitOfWorkOptions startupOptions = null)
        {
            ServiceCollection = new ServiceCollection();

            var configuration = GetIConfigurationRoot(Directory.GetCurrentDirectory());
            var connectionString = configuration.GetConnectionString("Default");

            _connection = new SqliteConnection(connectionString);
            _connection.Open();

            ServiceCollection.AddDbContext<BloggingDbContext>(
                options => options.UseSqlite(_connection)
            );

            ServiceCollection.Configure<UnitOfWorkOptions>(options =>
            {
                options.IsTransactional = startupOptions?.IsTransactional ?? true;
                options.IsolationLevel = startupOptions?.IsolationLevel;
                options.Scope = startupOptions?.Scope;
                options.Timeout = startupOptions?.Timeout;
                options.AsyncFlowOption = startupOptions?.AsyncFlowOption;
            });
            
            ServiceCollection.AddTransient<IDbContextProvider<BloggingDbContext>, DbContextProvider<BloggingDbContext>>();
            ServiceCollection.AddTransient<IUnitOfWork, EfCoreUnitOfWork>();
            ServiceCollection.AddTransient<IEfCoreTransactionStrategy, DbContextEfCoreTransactionStrategy>();
            ServiceCollection.AddTransient<IRepository<Blog, int>, EfCoreRepository<BloggingDbContext, Blog, int>>();
            ServiceCollection.AddTransient<IUnitOfWorkManager, UnitOfWorkManager>();
            ServiceCollection.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();

            ServiceCollection.AddScoped<IActiveDbContextsService, ActiveDbContextsService>();

            ServiceCollection.AddSingleton<IConfiguration>(configuration);

            _scope = ServiceProvider.CreateScope();

            var bloggingDbContext = _scope.ServiceProvider.GetService<BloggingDbContext>();
            bloggingDbContext.Database.EnsureCreated();
            
            SeedTestData(bloggingDbContext);
        }

        private void SeedTestData(BloggingDbContext context)
        {
            var blog1 = new Blog() { Name = "test-blog-1" };
            context.Blogs.Add(blog1);
            context.SaveChanges();

            var post1 = new Post
            {
                Blog = blog1,
                Name = "test-post-1-title"
            };
            context.Posts.Add(post1);
            context.SaveChanges();
        }

        public TService GetService<TService>()
        {
            return _scope.ServiceProvider.GetService<TService>();
        }

        public IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {      
            var config = new KeyValuePair<string, string>[]
            {
                //new KeyValuePair<string, string>("ConnectionStrings:Default", "DataSource=myshareddb;mode=memory;cache=shared")
                new KeyValuePair<string, string>("ConnectionStrings:Default", "DataSource=:memory:")
            };
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddInMemoryCollection(config)
                .AddEnvironmentVariables()
                .Build();
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }

            _scope?.Dispose();
        }

        public static EfCorePipeline Create(UnitOfWorkOptions startupOptions = null)
        {
            var pipeline = new EfCorePipeline();
            pipeline.Initialize(startupOptions);

            return pipeline;
        }
    }
}
