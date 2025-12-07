using MediatR;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Materials.Queries;

public class GetMaterialsBySubjectQueryHandler : IRequestHandler<GetMaterialsBySubjectQuery, List<MaterialDto>>
{
    private readonly IMaterialMongoDB _materialRepository;

    public GetMaterialsBySubjectQueryHandler(IMaterialMongoDB materialRepository)
    {
        _materialRepository = materialRepository;
    }

    public async Task<List<MaterialDto>> Handle(GetMaterialsBySubjectQuery request, CancellationToken cancellationToken)
    {
        var materials = await _materialRepository.GetBySubjectId(request.SubjectId);

        return materials?.Select(m => new MaterialDto
        {
            Id = m.Id.Value,
            SubjectId = m.SubjectId.Value,
            Name = m.Name.Value,
            Year = m.Year.Value,
            MaterialType = m.MaterialType.ToString(),
            Size = m.Size.Size,
            FilePath = m.FilePath.Value,
            UploadedAt = m.UploadedAt
        }).ToList() ?? new List<MaterialDto>();
    }
}
