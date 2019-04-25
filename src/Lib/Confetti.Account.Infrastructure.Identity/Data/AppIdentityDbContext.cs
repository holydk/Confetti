using Confetti.Account.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Account.Infrastructure.Identity.Data
{
    /// <summary>
    /// Represents identity context
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