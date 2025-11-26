using FIIT_folder.Api.Models;
using FluentValidation;

namespace FIIT_folder.Api.Validators;

public class CreateSubjectRequestValidator : AbstractValidator<CreateSubjectRequest>
{
    public  CreateSubjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .MaximumLength(50).WithMessage("Длина названия не более 50 символов");

        RuleFor(x => x.Semester)
            .InclusiveBetween(1, 8).WithMessage("Семестр должен быть от 1 до 8");
    }
}