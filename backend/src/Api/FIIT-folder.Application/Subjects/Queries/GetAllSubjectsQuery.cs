using MediatR;
using FIIT_folder.Application.DTOs;

namespace FIIT_folder.Application.Subjects.Queries;

public record GetAllSubjectsQuery : IRequest<List<SubjectDto>>;
