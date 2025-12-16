using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Application.DTOs;

public class RatingResultDto
{
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
    public RatingType? UserRating { get; set; }
}