using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Application.DTOs;

public class RatingResultDto
{
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public RatingType? UserRating { get; set; }
}