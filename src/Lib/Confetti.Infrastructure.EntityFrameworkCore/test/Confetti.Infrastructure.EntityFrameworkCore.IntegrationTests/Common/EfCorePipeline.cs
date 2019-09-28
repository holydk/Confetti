using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using Confetti.Domain.Uow;
using Confetti.Infrastructure.EntityFrameworkCore.IntegrationTests.Domain;
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

            ServiceCollection.AddSingleton<IConfiguration>(configuration);

            var connectionString = configuration.GetConnectionString("Default");

            _connection = new SqliteConnection(connectionString);
            _connection.Open();

            ServiceCollection.AddDbContext<BloggingDbContext>(
                options => options.UseSqlite(_connection)
            );

            ServiceCollection
                .AddConfettiDomain(options => 
                {
                    options.DefaultUnitOfWorkOptions.IsTransactional = startupOptions?.IsTransactional ?? true;
                    options.DefaultUnitOfWorkOptions.IsolationLevel = startupOptions?.IsolationLevel;
                    options.DefaultUnitOfWorkOptions.Scope = startupOptions?.Scope;
                    options.DefaultUnitOfWorkOptions.Timeout = startupOptions?.Timeout;
                    options.DefaultUnitOfWorkOptions.AsyncFlowOption = startupOptions?.AsyncFlowOption;
                })
                .AddEntityFrameworkCore()
                .AddDbContextProvider<BloggingDbContext>()
                .AddEfCoreEntityRepository<BloggingDbContext, Blog, int>();
                
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
