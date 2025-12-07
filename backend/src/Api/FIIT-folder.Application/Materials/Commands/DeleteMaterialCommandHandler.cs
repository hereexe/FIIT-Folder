using MediatR;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Materials.Commands;

public class DeleteMaterialCommandHandler : IRequestHandler<DeleteMaterialCommand, bool>
{
    private readonly IMaterialMongoDB _materialRepository;
    private readonly IFileStorageRepository _fileStorage;

    public DeleteMaterialCommandHandler(
        IMaterialMongoDB materialRepository,
        IFileStorageRepository fileStorage)
    {
        _materialRepository = materialRepository;
        _fileStorage = fileStorage;
    }

    public async Task<bool> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _materialRepository.GetByIdStudyMaterial(request.Id);
        if (material == null)
            throw new KeyNotFoundException($"Материал с ID '{request.Id}' не найден");

        await _fileStorage.DeleteFile(material.FilePath.Value);

        return await _materialRepository.DeleteStudyMaterial(request.Id);
    }
}
