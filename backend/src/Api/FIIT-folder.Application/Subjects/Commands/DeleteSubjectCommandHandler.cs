using MediatR;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Subjects.Commands;

public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, bool>
{
    private readonly ISubjectRepository _subjectRepository;

    public DeleteSubjectCommandHandler(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<bool> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var subject = await _subjectRepository.GetById(request.Id);
        if (subject == null)
            throw new KeyNotFoundException($"Предмет с ID '{request.Id}' не найден");

        return await _subjectRepository.Delete(request.Id);
    }
}
