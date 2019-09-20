using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Identity.Infrastructure.Application.AspNetIdentity.Data
{
    /// <summary>
    /// Represents a identity context.
    /// </summary>
    /// <typeparam name="ApplicationUser">application user</typeparam>
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Ctor

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {          
        }
            
        #endregion
    }
}