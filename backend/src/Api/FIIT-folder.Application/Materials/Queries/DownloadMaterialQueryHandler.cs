using MediatR;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Materials.Queries;

public class DownloadMaterialQueryHandler : IRequestHandler<DownloadMaterialQuery, DownloadMaterialResult?>
{
    private readonly IMaterialMongoDB _materialRepository;
    private readonly IFileStorageRepository _fileStorage;

    public DownloadMaterialQueryHandler(
        IMaterialMongoDB materialRepository,
        IFileStorageRepository fileStorage)
    {
        _materialRepository = materialRepository;
        _fileStorage = fileStorage;
    }

    public async Task<DownloadMaterialResult?> Handle(DownloadMaterialQuery request, CancellationToken cancellationToken)
    {
        var material = await _materialRepository.GetByIdStudyMaterial(request.Id);
        if (material == null)
            return null;

        var fileStream = await _fileStorage.GetFile(material.FilePath.Value);

        return new DownloadMaterialResult
        {
            FileStream = fileStream,
            FileName = material.Name.Value,
            ContentType = GetContentType(material.Name.Value)
        };
    }

    private static string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".txt" => "text/plain",
            ".zip" => "application/zip",
            ".rar" => "application/x-rar-compressed",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };
    }
}
