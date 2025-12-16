using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Enums;
using MediatR;

namespace FIIT_folder.Application.Ratings.Commands;

public record RateMaterialCommand(Guid MaterialId, Guid UserId, RatingType RatingType): IRequest<RatingResultDto>;