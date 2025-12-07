using MediatR;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Application.Materials.Commands;

public class UploadMaterialCommandHandler : IRequestHandler<UploadMaterialCommand, MaterialDto>
{
    private readonly IMaterialMongoDB _materialRepository;
    private readonly IFileStorageRepository _fileStorage;
    private readonly ISubjectRepository _subjectRepository;

    public UploadMaterialCommandHandler(
        IMaterialMongoDB materialRepository,
        IFileStorageRepository fileStorage,
        ISubjectRepository subjectRepository)
    {
        _materialRepository = materialRepository;
        _fileStorage = fileStorage;
        _subjectRepository = subjectRepository;
    }

    public async Task<MaterialDto> Handle(UploadMaterialCommand request, CancellationToken cancellationToken)
    {
        // Проверяем существование предмета
        var subject = await _subjectRepository.GetById(request.SubjectId);
        if (subject == null)
            throw new KeyNotFoundException($"Предмет с ID '{request.SubjectId}' не найден");

        // Парсим тип материала
        var materialType = Enum.Parse<MaterialType>(request.MaterialType, ignoreCase: true);

        // Проверяем, что тип материала разрешён для данного предмета
        if (!subject.HasMaterialType(materialType))
            throw new InvalidOperationException($"Тип материала '{materialType}' не разрешён для предмета '{subject.Name.Value}'");

        // Формируем путь в облаке: SubjectName/MaterialType/
        var folderPath = $"{subject.Name.Value}/{materialType}";

        // Сохраняем файл в облачное хранилище
        var filePath = await _fileStorage.SaveFile(
            request.FileName,
            request.Size,
            request.ContentType,
            request.FileStream,
            folderPath);

        // Создаём доменную сущность
        var material = new StudyMaterial(
            new MaterialName(request.FileName),
            new SubjectId(request.SubjectId),
            new UserId(request.UserId),
            new StudyYear(request.Year),
            new MaterialSize(request.Size),
            materialType,
            new ResourceLocation(filePath));

        // Сохраняем метаданные в MongoDB
        var created = await _materialRepository.CreateStudyMaterial(material);

        return new MaterialDto
        {
            Id = created.Id.Value,
            SubjectId = created.SubjectId.Value,
            Name = created.Name.Value,
            Year = created.Year.Value,
            MaterialType = created.MaterialType.ToString(),
            Size = created.Size.Size,
            FilePath = created.FilePath.Value,
            UploadedAt = created.UploadedAt
        };
    }
}
