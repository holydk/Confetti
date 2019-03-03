using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Confetti.Infrastructure.Data
{
    public class ConfettiContextFactory : IDesignTimeDbContextFactory<ConfettiContext>
    {
        public ConfettiContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ConfettiContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=confettidb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new ConfettiContext(optionsBuilder.Options);
        }
    }
}