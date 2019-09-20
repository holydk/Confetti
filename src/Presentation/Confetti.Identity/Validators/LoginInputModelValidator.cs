using Confetti.Identity.ViewModels;
using FluentValidation;

namespace Confetti.Identity.Validators
{
    public class LoginInputModelValidator : AbstractValidator<LoginInputModel>
    {
        public LoginInputModelValidator()
        {
            RuleFor(model => model.Email)
                .NotNull()
                .WithMessage("Упс... кажется, вы забыли ввести адрес электронной почты.")
                .NotEmpty()
                .WithMessage("Упс... кажется, вы забыли ввести адрес электронной почты.")
                .MaximumLength(255)
                .WithMessage("К сожалению, этот адрес электронной почты слишком длинный.");
            RuleFor(model => model.Password)
                .NotNull()
                .WithMessage("Ой... кажется, вы забыли ввести пароль.")
                .NotEmpty()
                .WithMessage("Ой... кажется, вы забыли ввести пароль.")
                .MaximumLength(100)
                .WithMessage("Погодите, слишком много символов. Менее 100 символов, пожалуйста.");
        }
    }
}