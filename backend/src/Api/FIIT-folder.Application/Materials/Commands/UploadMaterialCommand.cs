using MediatR;
using FIIT_folder.Application.DTOs;

namespace FIIT_folder.Application.Materials.Commands;

public record UploadMaterialCommand(
    Guid SubjectId,
    Guid UserId,
    string FileName,
    int Year,
    int Semester,
    string Description,
    string MaterialType,
    long Size,
    string ContentType,
    Stream FileStream) : IRequest<MaterialDto>;
