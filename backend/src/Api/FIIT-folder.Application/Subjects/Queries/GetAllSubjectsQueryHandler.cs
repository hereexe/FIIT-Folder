using MediatR;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Extensions;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Subjects.Queries;

public class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, List<SubjectDto>>
{
    private readonly ISubjectRepository _subjectRepository;

    public GetAllSubjectsQueryHandler(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<List<SubjectDto>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
    {
        var subjects = await _subjectRepository.GetAll();

        return subjects.Select(s => new SubjectDto
        {
            Id = s.Id.Value,
            Name = s.Name.Value,
            Semester = s.Semester.Value,
            MaterialTypes = s.AvailableMaterialTypes.Select(t => new MaterialTypeDto
            {
                Value = t.ToString(),
                DisplayName = t.GetDescription()
            }).ToList()
        }).ToList();
    }
}
