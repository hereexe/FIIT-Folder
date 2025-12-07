using MediatR;
using FIIT_folder.Application.DTOs;

namespace FIIT_folder.Application.Subjects.Commands;

public record CreateSubjectCommand(
    string Name,
    int Semester,
    List<string> MaterialTypes) : IRequest<SubjectDto>;
