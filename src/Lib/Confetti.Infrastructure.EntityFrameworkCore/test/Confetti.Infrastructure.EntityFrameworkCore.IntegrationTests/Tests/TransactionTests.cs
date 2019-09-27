using System;
using System.Linq;
using System.Threading.Tasks;
using Confetti.Domain.Repositories;
using Confetti.Domain.Uow;
using Confetti.Infrastructure.EntityFrameworkCore.IntegrationTests.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

#pragma warning disable 162

namespace Confetti.Infrastructure.EntityFrameworkCore.IntegrationTests.Tests
{
    public class TransactionTests
    {
        [Test]
        public async Task Exception_should_rollback_transaction()
        {
            const string exceptionMessage = "This is a test exception!";

            using (var pipeline = EfCorePipeline.Create())
            {
                try
                {
                    var unitOfWorkManager = pipeline.GetService<IUnitOfWorkManager>();
                    using (var uow = unitOfWorkManager.Begin())
                    {
                        var repository = pipeline.GetService<IRepository<Blog, int>>();
                        await repository.InsertAsync(new Blog() { Name = "blog1" });

                        throw new Exception(exceptionMessage);
                        await uow.CompleteAsync();
                    }
                }
                catch (Exception ex) when (ex.Message == exceptionMessage)
                {

                }

                var context = pipeline.GetService<BloggingDbContext>();
                var blog = await context.Blogs.FirstOrDefaultAsync(b => b.Name == "blog1");
                blog.Should().BeNull();
            }
        }

        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public async Task When_uow_is_disposed_the_dbcontext_should_not_contain_modified_entities(
            bool isTransactional, 
            bool isShouldComplete,
            bool shouldThrowException)
        {
            const string exceptionMessage = "This is a test exception!";

            var options = new UnitOfWorkOptions()
            {
                IsTransactional = isTransactional
            };
            using (var pipeline = EfCorePipeline.Create(options))
            {
                try
                {
                    var unitOfWorkManager = pipeline.GetService<IUnitOfWorkManager>();
                    using (var uow = unitOfWorkManager.Begin())
                    {
                        var repository = pipeline.GetService<IRepository<Blog, int>>();
                        await repository.InsertAsync(new Blog() { Name = "blog1" });

                        if (shouldThrowException)
                        {
                            throw new Exception(exceptionMessage);
                        }

                        if (isShouldComplete)
                        {
                            await uow.CompleteAsync();
                        }
                    }
                }
                catch (Exception ex) when (ex.Message == exceptionMessage)
                {

                }

                var context = pipeline.GetService<BloggingDbContext>();
                var entries = context.ChangeTracker.Entries().Where(
                    e => e.State == EntityState.Added
                          || e.State == EntityState.Modified
                              || e.State == EntityState.Deleted);  
                entries.Should().BeEmpty();
            }
        }

        [Test]
        [TestCase(true, null, 10, false)]
        [TestCase(true, 5, 10, false)]
        [TestCase(false, null, 10, false)]
        [TestCase(false, 5, 10, false)]
        [TestCase(true, null, 10, true)]
        [TestCase(true, 5, 10, true)]
        [TestCase(false, null, 10, true)]
        [TestCase(false, 5, 10, true)]
        public async Task When_uow_is_disposed_the_CommandTimeout_in_DbContext_should_have_previous_timeout(
            bool isTransactional,
            int? dbContextCommandTimeoutInSeconds,
            int? defaultUowCommandTimeoutInSeconds,
            bool shouldThrowException)
        {
            const string exceptionMessage = "This is a test exception!";

            var options = new UnitOfWorkOptions()
            {
                IsTransactional = isTransactional,
                Timeout = defaultUowCommandTimeoutInSeconds.HasValue
                             ? TimeSpan.FromSeconds(defaultUowCommandTimeoutInSeconds.Value)
                             : default(TimeSpan?)
            };
            using (var pipeline = EfCorePipeline.Create(options))
            {
                var context = pipeline.GetService<BloggingDbContext>();
                if (dbContextCommandTimeoutInSeconds.HasValue)
                {
                    context.Database.SetCommandTimeout(dbContextCommandTimeoutInSeconds);
                }
                else
                {
                    dbContextCommandTimeoutInSeconds = context.Database.GetCommandTimeout();
                }

                try
                {
                    var unitOfWorkManager = pipeline.GetService<IUnitOfWorkManager>();
                    using (var uow = unitOfWorkManager.Begin())
                    {
                        var repository = pipeline.GetService<IRepository<Blog, int>>();
                        await repository.InsertAsync(new Blog() { Name = "blog1" });

                        // if CommandTimeout is preinitialized
                        if (dbContextCommandTimeoutInSeconds.HasValue)
                        {
                            pipeline
                                .GetService<IDbContextProvider<BloggingDbContext>>()
                                .Get().Database.GetCommandTimeout().Should().Be(dbContextCommandTimeoutInSeconds);
                        }

                        if (shouldThrowException)
                        {
                            throw new Exception(exceptionMessage);
                        }

                        await uow.CompleteAsync();
                    }
                }
                catch (Exception ex) when (ex.Message == exceptionMessage)
                {

                }

                context.Database.GetCommandTimeout().Should().Be(dbContextCommandTimeoutInSeconds);
            }
        }
    }
}