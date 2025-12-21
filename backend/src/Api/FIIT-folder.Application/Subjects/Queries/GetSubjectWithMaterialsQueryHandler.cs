using MediatR;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Extensions;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Subjects.Queries;

public class GetSubjectWithMaterialsQueryHandler : IRequestHandler<GetSubjectWithMaterialsQuery, SubjectWithMaterialsDto?>
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly IMaterialMongoDB _materialRepository;

    public GetSubjectWithMaterialsQueryHandler(
        ISubjectRepository subjectRepository,
        IMaterialMongoDB materialRepository)
    {
        _subjectRepository = subjectRepository;
        _materialRepository = materialRepository;
    }

    public async Task<SubjectWithMaterialsDto?> Handle(GetSubjectWithMaterialsQuery request, CancellationToken cancellationToken)
    {
        var targetSubject = await _subjectRepository.GetById(request.SubjectId);
        if (targetSubject == null)
            return null;

        // Consolidate: find all semesters of this subject
        var subjects = await _subjectRepository.GetByName(targetSubject.Name.Value);
        
        var resultGroups = new List<MaterialGroupDto>();
        
        // Material types matrix
        var allMaterialTypes = subjects
            .SelectMany(s => s.AvailableMaterialTypes)
            .Distinct()
            .ToList();

        foreach (var materialType in allMaterialTypes)
        {
            var group = new MaterialGroupDto
            {
                ExamType = materialType.GetDescription(),
                RawType = materialType.ToString(),
                Items = new List<MaterialGroupItemDto>()
            };

            foreach (var s in subjects.OrderBy(sub => sub.Semester.Value))
            {
                if (s.HasMaterialType(materialType))
                {
                    group.Items.Add(new MaterialGroupItemDto
                    {
                        DisplayName = $"{GetSingularTypeDescription(materialType)}, {s.Semester.Value} семестр",
                        Semester = s.Semester.Value,
                        SubjectId = s.Id.Value
                    });
                }
            }

            if (group.Items.Any())
            {
                resultGroups.Add(group);
            }
        }

        return new SubjectWithMaterialsDto
        {
            Id = targetSubject.Id.Value,
            Name = targetSubject.Name.Value,
            MaterialGroups = resultGroups
        };
    }

    private string GetSingularTypeDescription(FIIT_folder.Domain.Enums.MaterialType type)
    {
        return type switch
        {
            FIIT_folder.Domain.Enums.MaterialType.Exam => "Экзамен",
            FIIT_folder.Domain.Enums.MaterialType.Colloquium => "Коллоквиум",
            FIIT_folder.Domain.Enums.MaterialType.Pass => "Зачёт",
            FIIT_folder.Domain.Enums.MaterialType.ControlWork => "Контрольная работа",
            FIIT_folder.Domain.Enums.MaterialType.ComputerPractice => "Компьютерный практикум",
            _ => type.ToString()
        };
    }
}
