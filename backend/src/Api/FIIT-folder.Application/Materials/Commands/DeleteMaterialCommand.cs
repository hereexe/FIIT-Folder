using MediatR;

namespace FIIT_folder.Application.Materials.Commands;

public record DeleteMaterialCommand(Guid Id) : IRequest<bool>;
