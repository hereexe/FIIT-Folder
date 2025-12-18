using FIIT_folder.Application.DTOs;
using MediatR;

namespace FIIT_folder.Application.Ratings.Queries;

public record GetRatingQuery(Guid MaterialId) : IRequest<RatingResultDto>;