using FIIT_folder.Api.Models;
using FIIT_folder.Domain.Entities;
using FluentValidation;

namespace FIIT_folder.Api.Validators;

public class CreateSubjectRequestValidator : AbstractValidator<CreateSubjectRequest>
{
    public CreateSubjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .MaximumLength(50).WithMessage("Длина названия не более 50 символов");

        RuleFor(x => x.Semester)
            .InclusiveBetween(1, 8).WithMessage("Семестр должен быть от 1 до 8");

        RuleFor(x => x.MaterialTypes)
            .NotEmpty().WithMessage("Необходимо указать хотя бы один тип материала")
            .Must(types => types.All(t => Enum.TryParse<MaterialType>(t, ignoreCase: true, out _)))
            .WithMessage("Указан неизвестный тип материала. Допустимые: Exam, Colloquium, Pass, ControlWork, ComputerPractice");
    }
}