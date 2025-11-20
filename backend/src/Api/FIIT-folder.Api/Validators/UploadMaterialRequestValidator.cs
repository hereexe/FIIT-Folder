using FIIT_folder.Api.Models;
using FluentValidation;

namespace FIIT_folder.Api.Validators;

public class UploadMaterialRequestValidator : AbstractValidator<UploadMaterialRequest>
{
    public UploadMaterialRequestValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Укажите название предмета или ID");

        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("Файл обязателен")
            .DependentRules(() =>
            {
                RuleFor(x => x.File!.Length)
                    .LessThanOrEqualTo(10 * 1024 * 1024) 
                    .WithMessage("Размер файла не должен превышать 10 МБ");

                RuleFor(x => x.File!.FileName)
                    .Must(HaveValidExtension)
                    .WithMessage("Недопустимый формат файла");
            });
    }

    private bool HaveValidExtension(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return false;
        }
        
        var allowed = new[] { ".pdf", ".docx", ".png", ".jpg" };
        var extension = Path.GetExtension(fileName).ToLower();
        
        return allowed.Contains(extension);
    }
}