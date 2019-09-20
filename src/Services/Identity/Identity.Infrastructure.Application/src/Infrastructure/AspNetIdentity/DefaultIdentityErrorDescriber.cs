using Microsoft.AspNetCore.Identity;

namespace Confetti.Identity.Infrastructure.Application.AspNetIdentity
{
    public class DefaultIdentityErrorDescriber : IdentityErrorDescriber
    {
        #region User

        public virtual IdentityError UserNotFound() => new IdentityError() { Code = nameof(UserNotFound), Description = "Хммм, кажется, такого аккаунта не существует." };
            
        #endregion

        #region Password

        public virtual IdentityError InvalidPassword() => new IdentityError() { Code = nameof(InvalidPassword), Description = "Похоже, вы неверно ввели пароль. Хотите попробовать еще раз?" };

        public virtual IdentityError NullOrEmptyPassword() => new IdentityError() { Code = nameof(NullOrEmptyPassword), Description = "Здесь требуется пароль." };

        public virtual IdentityError NullOrEmptyConfirmPassword() => new IdentityError() { Code = nameof(NullOrEmptyConfirmPassword), Description = "И еще разок пароль." };

        public override IdentityError PasswordMismatch() => new IdentityError() { Code = nameof(PasswordMismatch), Description = "Что то пароли не сходятся, попробуйте ввести еще раз." };

        public override IdentityError PasswordTooShort(int length) => new IdentityError() { Code = nameof(PasswordTooShort), Description = $"Хммм, чего-то не хватает, нужно ввести хотя бы {length} знаков." };

        public virtual IdentityError PasswordTooLong(int length) => new IdentityError() { Code = nameof(PasswordTooLong), Description = $"Погодите, слишком много символов. Менее {length} символов, пожалуйста." };
            
        #endregion

        #region First name

        public virtual IdentityError NullOrEmptyFirstName() => new IdentityError() { Code = nameof(NullOrEmptyFirstName), Description = "Давайте знакомиться - ваше имя?" };

        public virtual IdentityError FirstNameTooLong(int length) => new IdentityError() { Code = nameof(FirstNameTooLong), Description = $"Хм, здесь должно быть менее {length} символов." };

        public virtual IdentityError FirstNameInvalidSymbols(string symbols) => new IdentityError() { Code = nameof(FirstNameInvalidSymbols), Description = $"Внутренний перфекционист считает, что не мешало бы исключить такие символы как {symbols}" };

            
        #endregion

        #region Last name

        public virtual IdentityError NullOrEmptyLastName() => new IdentityError() { Code = nameof(NullOrEmptyLastName), Description = "Фамилия тоже не помешает!" };

        public virtual IdentityError LastNameTooLong(int length) => new IdentityError() { Code = nameof(LastNameTooLong), Description = $"Хм, здесь должно быть менее {length} символов." };

        public virtual IdentityError LastNameInvalidSymbols(string symbols) => new IdentityError() { Code = nameof(LastNameInvalidSymbols), Description = $"Внутренний перфекционист считает, что не мешало бы исключить такие символы как {symbols}" };
            
        #endregion

        #region Email

        public virtual IdentityError NullOrEmptyEmail() => new IdentityError() { Code = nameof(NullOrEmptyEmail), Description = "Ой! Здесь вам надо указать адрес электронной почты." };

        public override IdentityError InvalidEmail(string email) => new IdentityError() { Code = nameof(InvalidEmail), Description = "Введенный адрес электронной почты не верен! Пожалуйста, укажите свой правильный адрес электронной почты." };
            
        #endregion
    }
}