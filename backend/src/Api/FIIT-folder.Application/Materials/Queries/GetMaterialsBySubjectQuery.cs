using MediatR;
using FIIT_folder.Application.DTOs;

namespace FIIT_folder.Application.Materials.Queries;

public record GetMaterialsBySubjectQuery(Guid? SubjectId, Guid? UserId = null, int? Semester = null, int? Year = null, string? SearchText = null) : IRequest<List<MaterialDto>>;
