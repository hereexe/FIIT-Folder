using MediatR;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Enums;
using FIIT_folder.Domain.Extensions;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Application.Subjects.Commands;

public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, SubjectDto>
{
    private readonly ISubjectRepository _subjectRepository;

    public CreateSubjectCommandHandler(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<SubjectDto> Handle(CreateSubjectCommand request, CancellationToken cancellationToken)
    {
        var existsByName = await _subjectRepository.ExistsByName(request.Name);
        if (existsByName)
            throw new InvalidOperationException($"Предмет с именем '{request.Name}' уже существует");

        var materialTypes = request.MaterialTypes
            .Select(t => Enum.Parse<MaterialType>(t, ignoreCase: true))
            .ToList();

        var subject = new Subject(
            SubjectId.New(),
            new SubjectName(request.Name),
            new SubjectSemester(request.Semester),
            materialTypes);

        var created = await _subjectRepository.Create(subject);

        return new SubjectDto
        {
            Id = created.Id.Value,
            Name = created.Name.Value,
            Semester = created.Semester.Value,
            MaterialTypes = created.AvailableMaterialTypes.Select(t => new MaterialTypeDto
            {
                Value = t.ToString(),
                DisplayName = t.GetDescription()
            }).ToList()
        };
    }
}
