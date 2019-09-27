using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.EntityFrameworkCore.IntegrationTests.Domain
{
    public class BloggingDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        
        public BloggingDbContext(DbContextOptions<BloggingDbContext> options)
            : base(options)
        {
            
        }
    }
}