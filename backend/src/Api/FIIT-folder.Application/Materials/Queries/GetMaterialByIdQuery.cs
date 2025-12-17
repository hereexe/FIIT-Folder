using MediatR;
using FIIT_folder.Application.DTOs;

namespace FIIT_folder.Application.Materials.Queries;

public record GetMaterialByIdQuery(Guid Id, Guid? UserId = null) : IRequest<MaterialDto?>;
