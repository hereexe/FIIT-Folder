using FIIT_folder.Api.Models;
using FIIT_folder.Domain.Entities;
using FluentValidation;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Api.Validators;

public class UploadMaterialRequestValidator : AbstractValidator<UploadMaterialRequest>
{
    private static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".png", ".jpg", ".jpeg", ".ppt", ".pptx", ".txt" };

    public UploadMaterialRequestValidator()
    {
        RuleFor(x => x.SubjectId)
            .NotEmpty().WithMessage("Укажите ID предмета");

        RuleFor(x => x.Year)
            .InclusiveBetween(2019, DateTime.UtcNow.Year + 1).WithMessage("Год должен быть валидным");

        RuleFor(x => x.MaterialType)
            .NotEmpty().WithMessage("Укажите тип материала")
            .Must(type => Enum.TryParse<MaterialType>(type, ignoreCase: true, out _))
            .WithMessage("Неизвестный тип материала. Допустимые: Exam, Colloquium, Pass, ControlWork, ComputerPractice");

        RuleFor(x => x.File)
            .NotNull().WithMessage("Файл обязателен")
            .DependentRules(() =>
            {
                RuleFor(x => x.File!.Length)
                    .GreaterThan(0).WithMessage("Файл пустой")
                    .LessThanOrEqualTo(25 * 1024 * 1024).WithMessage("Размер файла не должен превышать 25 МБ");

                RuleFor(x => x.File!.FileName)
                    .Must(HaveValidExtension)
                    .WithMessage($"Недопустимый формат файла. Разрешены: {string.Join(", ", AllowedExtensions)}");
            });
    }

    private static bool HaveValidExtension(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return false;
        
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return AllowedExtensions.Contains(extension);
    }
}