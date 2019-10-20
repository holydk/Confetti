using Confetti.Infrastructure.EntityFrameworkCore.SqlServer.Design;

namespace Confetti.Identity.Infrastructure.Application.AspNetIdentity.Data
{
    /// <summary>
    /// Represents a factory to create <see cref="AppIdentityDbContext" />.
    /// </summary>
    public class AppIdentityDbContextFactory : SqlServerConfettiContextFactory<AppIdentityDbContext>
    {

    }
}