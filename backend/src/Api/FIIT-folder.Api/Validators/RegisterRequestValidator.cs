using FIIT_folder.Api.Models;
using FluentValidation;

namespace FIIT_folder.Api.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username обязателен")
            .Length(3, 20)
            .Matches("^[a-zA-Z0-9._]+$")
            .WithMessage("Недопустимые символы в username");


        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(6).WithMessage("Пароль должен быть длиннее 6 символов");
    }
}