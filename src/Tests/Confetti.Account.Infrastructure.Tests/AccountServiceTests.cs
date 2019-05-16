using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Confetti.Account.Application.Models;
using Confetti.Account.Infrastructure.Identity.Models;
using Confetti.Common;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace Confetti.Account.Infrastructure.Tests
{
    [TestFixture]
    public class AccountServiceTests
    {
        #region Utilities

        private Task<ActionResult> TryRegisterAsync(
            string email, 
            string firstName, 
            string lastName, 
            string password, 
            string confirmPassword,
            Action<Mock<UserManager<ApplicationUser>>> configureMockUserManager = null,
            Action<Mock<RoleManager<IdentityRole>>> configureMockRoleManager = null
        )
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                UserName = email
            };

            var mockUserManager = MockHelpers.MockUserManager<ApplicationUser>();
            
            if (configureMockUserManager == null)
            {
                SetUpMockUserManager(mockUserManager, user, password);
            }
            else
            {
                configureMockUserManager?.Invoke(mockUserManager);
            }

            var mockRoleManager = MockHelpers.MockRoleManager<IdentityRole>();
            configureMockRoleManager?.Invoke(mockRoleManager);
            
            var accountService = MockHelpers.TestAccountService(
                mockUserManager.Object, mockRoleManager.Object);

            var model = new RegisterInputModel()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            return accountService.RegisterAsync(model);
        }

        private Task<ActionResult> TryValidateCredentialsAsync(
            string email,
            string password,
            Action<Mock<UserManager<ApplicationUser>>> configureMockUserManager = null,
            Action<Mock<RoleManager<IdentityRole>>> configureMockRoleManager = null
        )
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = email
            };

            var mockUserManager = MockHelpers.MockUserManager<ApplicationUser>();
            
            if (configureMockUserManager == null)
            {
                SetUpMockUserManager(mockUserManager, user, password);
                mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                    .ReturnsAsync(true);
            }
            else
            {
                configureMockUserManager?.Invoke(mockUserManager);
            }

            var mockRoleManager = MockHelpers.MockRoleManager<IdentityRole>();
            configureMockRoleManager?.Invoke(mockRoleManager);
            
            var accountService = MockHelpers.TestAccountService(
                mockUserManager.Object, mockRoleManager.Object);

            return accountService.ValidateCredentialsAsync(email, password);
        }

        private void SetUpMockUserManager(
            Mock<UserManager<ApplicationUser>> mockUserManager,
            ApplicationUser user,
            string password)
        {
            mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(user.Id);
            mockUserManager.Setup(um => um.GetUserIdAsync(It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult(user.Id));
            mockUserManager.Setup(um => um.GetUserName(It.IsAny<ClaimsPrincipal>()))
                .Returns(user.UserName);
            mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            mockUserManager.Setup(um => um.GetUserNameAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(user.UserName);
            mockUserManager.Setup(um => um.GetEmailAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(user.Email);
            mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(ValidateUser(user, mockUserManager.Object, password));
        }

        private IdentityResult ValidateUser(
            ApplicationUser user,
            UserManager<ApplicationUser> userManager,
            string password)
        {
            var errors = new List<IdentityError>();
            foreach (var validator in userManager.UserValidators)
            {
                var result = validator.ValidateAsync(userManager, user)
                    .GetAwaiter()
                    .GetResult();
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            foreach (var validator in userManager.PasswordValidators)
            {
                var result = validator.ValidateAsync(userManager, user, password)
                    .GetAwaiter()
                    .GetResult();
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                } 
            }
    
            return (errors.Count > 0) 
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;
        }
            
        #endregion

        #region Methods

        #region Register Tests

        [Test]
        public async Task RegisterTest_Normal()
        {
            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test",
                lastName: "test",
                password: "goodPassword",
                confirmPassword: "goodPassword"
            );

            Assert.IsTrue(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        #region Email Tests
            
        [Test]
        public async Task RegisterTest_NullEmail()
        {
            var result = await TryRegisterAsync(
                email: null,
                firstName: "test",
                lastName: "test",
                password: "goodPassword",
                confirmPassword: "goodPassword"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task RegisterTest_InvalidEmail()
        {
            var result = await TryRegisterAsync(
                email: "InvalidEmail",
                firstName: "test",
                lastName: "test",
                password: "goodPassword",
                confirmPassword: "goodPassword"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }
        
        [Test]
        public async Task RegisterTest_DuplicateEmail()
        {
            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test",
                lastName: "test",
                password: "goodPassword",
                confirmPassword: "goodPassword",
                configureMockUserManager: mockUserManager =>
                {
                    var userPassword = "goodPassword";
                    var user = new ApplicationUser() 
                    { 
                        Id = "userId", 
                        Email = "test@test.ru", 
                        FirstName = "test", 
                        LastName = "test"
                    };
                    var anotherUser = new ApplicationUser() 
                    { 
                        Id = "anotherUserId", 
                        Email = "test@test.ru", 
                        FirstName = "test", 
                        LastName = "test"
                    };
                    mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                        .Returns(anotherUser.Id);
                    mockUserManager.Setup(um => um.GetUserIdAsync(It.IsAny<ApplicationUser>()))
                        .Returns(Task.FromResult(anotherUser.Id));
                    mockUserManager.Setup(um => um.GetUserName(It.IsAny<ClaimsPrincipal>()))
                        .Returns(user.UserName);
                    mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                        .ReturnsAsync(user);
                    mockUserManager.Setup(um => um.GetUserNameAsync(It.IsAny<ApplicationUser>()))
                        .ReturnsAsync(user.UserName);
                    mockUserManager.Setup(um => um.GetEmailAsync(It.IsAny<ApplicationUser>()))
                        .ReturnsAsync(anotherUser.Email);
                    mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync(anotherUser);
                    mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(ValidateUser(user, mockUserManager.Object, userPassword));
                }
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        #endregion
            
        #region FirstName Tests
            
        [Test]
        public async Task RegisterTest_NullFirstName()
        {
            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: null,
                lastName: "test",
                password: "goodPassword",
                confirmPassword: "goodPassword"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task RegisterTest_TooLongFirstName()
        {
            var firstName = string.Empty;
            for (int i = 0; i < 26; i++)
            {
                firstName += "p";
            }

            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: firstName,
                lastName: "test",
                password: "goodPassword",
                confirmPassword: "goodPassword"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task RegisterTest_FirstNameInvalidSymbols()
        {
            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test.>",
                lastName: "test",
                password: "goodPassword",
                confirmPassword: "goodPassword"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }
        
        #endregion

        #region LastName Tests
            
        [Test]
        public async Task RegisterTest_NullLastName()
        {
            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test",
                lastName: null,
                password: "goodPassword",
                confirmPassword: "goodPassword"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task RegisterTest_TooLongLastName()
        {
            var lastName = string.Empty;
            for (int i = 0; i < 26; i++)
            {
                lastName += "p";
            }

            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test",
                lastName: lastName,
                password: "goodPassword",
                confirmPassword: "goodPassword"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task RegisterTest_LastNameInvalidSymbols()
        {
            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test",
                lastName: "test\"<&",
                password: "goodPassword",
                confirmPassword: "goodPassword"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        #endregion

        #region Password Tests
   
        [Test]
        public async Task RegisterTest_NullPassword()
        {
            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test",
                lastName: "test",
                password: null,
                confirmPassword: "goodPassword",
                configureMockUserManager: mockUserManager =>
                {
                    mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);
                }
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task RegisterTest_NullConfirmPassword()
        {
            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test",
                lastName: "test",
                password: "goodPassword",
                confirmPassword: null
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task RegisterTest_PasswordNotEqualsConfirmPassword()
        {
            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test",
                lastName: "test",
                password: "goodPassword",
                confirmPassword: "anotherPassword"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task RegisterTest_SmallPassword()
        {
            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test",
                lastName: "test",
                password: "small",
                confirmPassword: "small"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task RegisterTest_TooLongPassword()
        {
            var password = string.Empty;
            for (int i = 0; i < 101; i++)
            {
                password += "p";
            }

            var result = await TryRegisterAsync(
                email: "test@test.ru",
                firstName: "test",
                lastName: "test",
                password: password,
                confirmPassword: password
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }
            
        #endregion

        #endregion

        #region Validate credentials Tests
        
        [Test]
        public async Task ValidateCredentialsTest_Normal()
        {
            var result = await TryValidateCredentialsAsync(
                "test@test.ru",
                "goodPassword"
            );

            Assert.IsTrue(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task ValidateCredentialsTest_NullEmail()
        {
            var result = await TryValidateCredentialsAsync(
                null,
                "goodPassword"
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task ValidateCredentialsTest_NullPassword()
        {
            var result = await TryValidateCredentialsAsync(
                "test@test.ru",
                null,
                configureMockUserManager: mockUserManager =>
                {
                    mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);
                }
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        [Test]
        public async Task ValidateCredentialsTest_UserNotFound()
        {
            var result = await TryValidateCredentialsAsync(
                "test@test.ru",
                "goodPassword",
                configureMockUserManager: mockUserManager => 
                {
                    mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync(It.IsAny<ApplicationUser>());
                    mockUserManager.Setup(um => um.GetEmailAsync(It.IsAny<ApplicationUser>()))
                        .ReturnsAsync(It.IsAny<string>());
                    mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(false);
                }
            );

            Assert.IsFalse(result.Succeeded, string.Join(',', result.Errors.Select(e => e.Message)));
        }

        #endregion

        #endregion
    }
}