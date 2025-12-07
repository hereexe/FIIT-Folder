using MediatR;

namespace FIIT_folder.Application.Materials.Queries;

public record DownloadMaterialQuery(Guid Id) : IRequest<DownloadMaterialResult?>;

public class DownloadMaterialResult
{
    public Stream FileStream { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/octet-stream";
}
