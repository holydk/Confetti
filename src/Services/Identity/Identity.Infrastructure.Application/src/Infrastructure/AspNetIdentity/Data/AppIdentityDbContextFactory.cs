using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Confetti.Identity.Infrastructure.Application.AspNetIdentity.Data
{
    /// <summary>
    /// Represents a factory to create the identity context.
    /// </summary>
    public class AppIdentityDbContextFactory : IDesignTimeDbContextFactory<AppIdentityDbContext>
    {
        public AppIdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppIdentityDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=confettidb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new AppIdentityDbContext(optionsBuilder.Options);
        }
    }
}