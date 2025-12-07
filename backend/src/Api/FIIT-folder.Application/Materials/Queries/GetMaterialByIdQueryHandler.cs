using MediatR;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Materials.Queries;

public class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, MaterialDto?>
{
    private readonly IMaterialMongoDB _materialRepository;

    public GetMaterialByIdQueryHandler(IMaterialMongoDB materialRepository)
    {
        _materialRepository = materialRepository;
    }

    public async Task<MaterialDto?> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
    {
        var material = await _materialRepository.GetByIdStudyMaterial(request.Id);

        if (material == null)
            return null;

        return new MaterialDto
        {
            Id = material.Id.Value,
            SubjectId = material.SubjectId.Value,
            Name = material.Name.Value,
            Year = material.Year.Value,
            MaterialType = material.MaterialType.ToString(),
            Size = material.Size.Size,
            FilePath = material.FilePath.Value,
            UploadedAt = material.UploadedAt
        };
    }
}
