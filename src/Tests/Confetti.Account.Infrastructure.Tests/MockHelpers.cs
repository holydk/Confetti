using System.Collections.Generic;
using System.Threading.Tasks;
using Confetti.Account.Application.Configuration;
using Confetti.Account.Infrastructure.Identity;
using Confetti.Account.Infrastructure.Identity.Models;
using Confetti.Account.Infrastructure.Identity.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Confetti.Account.Infrastructure.Tests
{
    public static class MockHelpers
    {
        public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) 
            where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<TUser>>();
            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());
            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new DefaultIdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return userManager;
        }

        public static RoleManager<TRole> TestRoleManager<TRole>(IRoleStore<TRole> store = null) 
            where TRole : class
        {
            store = store ?? new Mock<IRoleStore<TRole>>().Object;
            var roles = new List<IRoleValidator<TRole>>();
            roles.Add(new RoleValidator<TRole>());
            return new AspNetRoleManager<TRole>(store, roles,
                new UpperInvariantLookupNormalizer(),
                new DefaultIdentityErrorDescriber(),
                null,
                null);
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>() 
            where TUser : ApplicationUser
        {
            var store = new Mock<IUserPasswordStore<TUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            var mockAccountOptions = new Mock<IOptions<AccountOptions>>();
            var accountOptions = new AccountOptions();
            mockAccountOptions.Setup(o => o.Value).Returns(accountOptions);
            idOptions.User.RequireUniqueEmail = accountOptions.RequireUniqueEmail;
            idOptions.Password.RequireDigit = accountOptions.RequireDigitPassword;
            idOptions.Password.RequireLowercase = accountOptions.RequireLowercasePassword;
            idOptions.Password.RequireNonAlphanumeric = accountOptions.RequireNonAlphanumericPassword;
            idOptions.Password.RequireUppercase = accountOptions.RequireUppercasePassword;
            idOptions.Password.RequiredLength = accountOptions.RequiredPasswordLength;
            idOptions.Lockout.MaxFailedAccessAttempts = accountOptions.MaxFailedAccessAttempts;
            idOptions.Lockout.DefaultLockoutTimeSpan = accountOptions.DefaultLockoutTimeSpan;
            options.Setup(o => o.Value).Returns(idOptions);
            var mgr = new Mock<UserManager<TUser>>(store.Object, options.Object, null, null, null, null, new DefaultIdentityErrorDescriber(), null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.UserValidators.Add(new DefaultUserValidator<TUser>(mockAccountOptions.Object));
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new DefaultPasswordValidator<TUser>(mockAccountOptions.Object));
            mgr.Setup(um => um.AddToRoleAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return mgr;
        }

        public static Mock<RoleManager<TRole>> MockRoleManager<TRole>(IRoleStore<TRole> store = null) 
            where TRole : class
        {
            store = store ?? new Mock<IRoleStore<TRole>>().Object;
            var roles = new List<IRoleValidator<TRole>>();
            roles.Add(new RoleValidator<TRole>());
            return new Mock<RoleManager<TRole>>(store, roles, new UpperInvariantLookupNormalizer(),
                new DefaultIdentityErrorDescriber(), null);
        }

        public static AccountService TestAccountService(
            UserManager<ApplicationUser> userManager = null,
            RoleManager<IdentityRole> roleManager = null)
        {
            userManager = userManager ?? MockUserManager<ApplicationUser>().Object;
            roleManager = roleManager ?? MockRoleManager<IdentityRole>().Object;

            return new AccountService(userManager, roleManager, new DefaultIdentityErrorDescriber());
        }
    }
}