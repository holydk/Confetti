using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Confetti.Account.Infrastructure.Identity.Data
{
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