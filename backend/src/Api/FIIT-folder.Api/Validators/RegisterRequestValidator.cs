using FIIT_folder.Api.Models;
using FluentValidation;

namespace FIIT_folder.Api.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Логин обязателен")
            .Length(4, 20).WithMessage("Логин должен быть от 4 до 20 символов")
            .Matches(@"^[a-zA-Z0-9._а-яА-ЯёЁ]+$")
            .WithMessage("Логин может содержать только буквы (латиница/кириллица), цифры, точки и подчёркивания");


        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль обязателен")
            .MinimumLength(6).WithMessage("Пароль должен быть длиннее 6 символов");
    }
}