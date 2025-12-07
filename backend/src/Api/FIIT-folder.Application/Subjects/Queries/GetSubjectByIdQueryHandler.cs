using MediatR;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Extensions;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Subjects.Queries;

public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, SubjectDto?>
{
    private readonly ISubjectRepository _subjectRepository;

    public GetSubjectByIdQueryHandler(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<SubjectDto?> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
    {
        var subject = await _subjectRepository.GetById(request.Id);

        if (subject == null)
            return null;

        return new SubjectDto
        {
            Id = subject.Id.Value,
            Name = subject.Name.Value,
            Semester = subject.Semester.Value,
            MaterialTypes = subject.AvailableMaterialTypes.Select(t => new MaterialTypeDto
            {
                Value = t.ToString(),
                DisplayName = t.GetDescription()
            }).ToList()
        };
    }
}
