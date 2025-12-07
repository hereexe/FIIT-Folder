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
        var subject = await _subjectRepository.GetById(request.SubjectId);
        if (subject == null)
            return null;

        var materials = await _materialRepository.GetBySubjectId(request.SubjectId);

        var materialGroups = subject.AvailableMaterialTypes
            .Select(materialType => new MaterialGroupDto
            {
                ExamType = materialType.GetDescription(),
                ExamNames = materials
                    .Where(m => m.MaterialType == materialType)
                    .Select(m => m.Name.Value)
                    .Distinct()
                    .OrderBy(name => name)
                    .ToList()
            })
            .Where(g => g.ExamNames.Count > 0)
            .ToList();

        return new SubjectWithMaterialsDto
        {
            Id = subject.Id.Value,
            Name = subject.Name.Value,
            MaterialGroups = materialGroups
        };
    }
}
