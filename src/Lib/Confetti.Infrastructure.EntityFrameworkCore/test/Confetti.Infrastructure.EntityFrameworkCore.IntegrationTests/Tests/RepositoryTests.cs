using System.Threading.Tasks;
using Confetti.Domain.Repositories;
using Confetti.Domain.Uow;
using Confetti.Infrastructure.EntityFrameworkCore.IntegrationTests.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Confetti.Infrastructure.EntityFrameworkCore.IntegrationTests.Tests
{
    public class Repository_Tests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Complited_uow_should_add_entity(bool isTransactional)
        {
            var options = new UnitOfWorkOptions()
            {
                IsTransactional = isTransactional
            };
            using (var pipeline = EfCorePipeline.Create(options))
            {
                var unitOfWorkManager = pipeline.GetService<IUnitOfWorkManager>();
                using (var uow = unitOfWorkManager.Begin())
                {
                    var repository = pipeline.GetService<IRepository<Blog, int>>();
                    await repository.InsertAsync(new Blog() { Name = "blog1" });

                    await uow.CompleteAsync();
                }

                var context = pipeline.GetService<BloggingDbContext>();
                var blog = await context.Blogs.FirstOrDefaultAsync(b => b.Name == "blog1");
                blog.Should().NotBeNull();
            }
        }
    
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task UnComplited_uow_should_not_add_entity(bool isTransactional)
        {
            var options = new UnitOfWorkOptions()
            {
                IsTransactional = isTransactional
            };
            using (var pipeline = EfCorePipeline.Create(options))
            {
                var unitOfWorkManager = pipeline.GetService<IUnitOfWorkManager>();
                using (var uow = unitOfWorkManager.Begin())
                {
                    var repository = pipeline.GetService<IRepository<Blog, int>>();
                    await repository.InsertAsync(new Blog() { Name = "blog1" });
                }

                var context = pipeline.GetService<BloggingDbContext>();
                var blog = await context.Blogs.FirstOrDefaultAsync(b => b.Name == "blog1");
                blog.Should().BeNull();
            }
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        public async Task Complited_uow_should_update_entity(bool isTransactional, bool shouldCallUpdate)
        {
            var options = new UnitOfWorkOptions()
            {
                IsTransactional = isTransactional
            };
            using (var pipeline = EfCorePipeline.Create(options))
            {
                var context = pipeline.GetService<BloggingDbContext>();
                var unitOfWorkManager = pipeline.GetService<IUnitOfWorkManager>();
                using (var uow = unitOfWorkManager.Begin())
                {
                    var repository = pipeline.GetService<IRepository<Blog, int>>();

                    var existBlog = await repository.GetAll()
                        .FirstOrDefaultAsync(b => b.Name == "test-blog-1");
                    existBlog.Name = "updated-test-blog-1";

                    if (shouldCallUpdate)
                    {
                        await repository.UpdateAsync(existBlog);
                    }

                    await uow.CompleteAsync();
                }

                
                var blog = await context.Blogs.FirstOrDefaultAsync(b => b.Name == "updated-test-blog-1");
                blog.Should().NotBeNull();
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task Complited_uow_should_delete_entity(bool isTransactional)
        {
            var options = new UnitOfWorkOptions()
            {
                IsTransactional = isTransactional
            };
            using (var pipeline = EfCorePipeline.Create(options))
            {
                var context = pipeline.GetService<BloggingDbContext>();
                var unitOfWorkManager = pipeline.GetService<IUnitOfWorkManager>();
                using (var uow = unitOfWorkManager.Begin())
                {
                    var repository = pipeline.GetService<IRepository<Blog, int>>();

                    var existBlog = await repository.GetAll()
                        .FirstOrDefaultAsync(b => b.Name == "test-blog-1");

                    await repository.DeleteAsync(existBlog);

                    await uow.CompleteAsync();
                }
                
                var blog = await context.Blogs.FirstOrDefaultAsync(b => b.Name == "test-blog-1");
                blog.Should().BeNull();
            }
        }
    }
}