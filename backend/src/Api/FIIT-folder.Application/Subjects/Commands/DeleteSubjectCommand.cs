using MediatR;

namespace FIIT_folder.Application.Subjects.Commands;

public record DeleteSubjectCommand(Guid Id) : IRequest<bool>;
