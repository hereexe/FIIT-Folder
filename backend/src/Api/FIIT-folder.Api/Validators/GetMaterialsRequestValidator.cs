using FIIT_folder.Api.Models;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Enums;
using FluentValidation;

namespace FIIT_folder.Api.Validators;

public class GetMaterialsRequestValidator : AbstractValidator<GetMaterialsRequest>
{
    public GetMaterialsRequestValidator()
    {
        RuleFor(x => x.SubjectId)
            .NotEmpty().WithMessage("SubjectId обязателен");

        RuleFor(x => x.MaterialType)
            .Must(type => string.IsNullOrEmpty(type) || Enum.TryParse<MaterialType>(type, true, out _))
            .WithMessage("Некорректный тип материала");
    }
}
