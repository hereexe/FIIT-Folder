using MediatR;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Materials.Queries;

public class GetMaterialsBySubjectQueryHandler : IRequestHandler<GetMaterialsBySubjectQuery, List<MaterialDto>>
{
    private readonly IMaterialMongoDB _materialRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IMaterialRatingRepository _ratingRepository;

    public GetMaterialsBySubjectQueryHandler(
        IMaterialMongoDB materialRepository,
        IUserRepository userRepository,
        IFavoriteRepository favoriteRepository,
        IMaterialRatingRepository ratingRepository)
    {
        _materialRepository = materialRepository;
        _userRepository = userRepository;
        _favoriteRepository = favoriteRepository;
        _ratingRepository = ratingRepository;
    }

    public async Task<List<MaterialDto>> Handle(GetMaterialsBySubjectQuery request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.StudyMaterial> materials;
        if (request.SubjectId.HasValue)
        {
            materials = await _materialRepository.GetBySubjectId(request.SubjectId.Value);
        }
        else if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            materials = await _materialRepository.SearchAsync(request.SearchText);
        }
        else
        {
            materials = await _materialRepository.GetAll();
        }

        if (materials == null)
            return new List<MaterialDto>();

        // Filter by semester if provided
        if (request.Semester.HasValue)
        {
            materials = materials.Where(m => m.Semester.Value == request.Semester.Value).ToList();
        }

        // Filter by year if provided
        if (request.Year.HasValue)
        {
            materials = materials.Where(m => m.Year.Value == request.Year.Value).ToList();
        }

        // Filter by text (Name or Description) if provided
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var searchText = request.SearchText.ToLower();
            materials = materials.Where(m => 
                (m.Name.Value?.ToLower().Contains(searchText) ?? false) || 
                (m.Description?.ToLower().Contains(searchText) ?? false)
            ).ToList();
        }

        // Get favorites for current user to check IsFavorite status in bulk logic if possible
        var favoriteMaterialIds = new HashSet<Guid>();
        if (request.UserId.HasValue)
        {
            var favorites = await _favoriteRepository.GetMaterialsByUserIdAsync(new FIIT_folder.Domain.Value_Object.UserId(request.UserId.Value), cancellationToken);
            foreach (var fav in favorites)
            {
                favoriteMaterialIds.Add(fav.MaterialId.Value);
            }
        }

        var result = new List<MaterialDto>();
        foreach (var m in materials)
        {
            var authorName = "Unknown";
            // Potential N+1 issue, but acceptable for now
            var user = await _userRepository.GetByIdAsync(m.UserId.Value, cancellationToken);
            if (user != null)
                authorName = user.Login.Value;

            var (likes, dislikes) = await _ratingRepository.GetRatingCountsAsync(m.Id.Value, cancellationToken);
            string? currentUserRating = null;
            if (request.UserId.HasValue)
            {
                var rating = await _ratingRepository.GetByUserAndMaterialAsync(
                    new FIIT_folder.Domain.Value_Object.UserId(request.UserId.Value),
                    m.Id);
                currentUserRating = rating?.Rating.ToString();
            }

            result.Add(new MaterialDto
            {
                Id = m.Id.Value,
                SubjectId = m.SubjectId.Value,
                Name = m.Name.Value,
                Year = m.Year.Value,
                Semester = m.Semester.Value,
                Description = m.Description,
                AuthorName = authorName,
                IsFavorite = favoriteMaterialIds.Contains(m.Id.Value),
                MaterialType = m.MaterialType.ToString(),
                Size = m.Size.Size,
                FilePath = m.FilePath.Value,
                UploadedAt = m.UploadedAt,
                LikesCount = likes,
                DislikesCount = dislikes,
                CurrentUserRating = currentUserRating
            });
        }

        return result;
    }
}
