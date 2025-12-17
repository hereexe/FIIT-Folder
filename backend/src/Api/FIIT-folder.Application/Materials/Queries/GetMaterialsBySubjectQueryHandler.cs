using MediatR;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Materials.Queries;

public class GetMaterialsBySubjectQueryHandler : IRequestHandler<GetMaterialsBySubjectQuery, List<MaterialDto>>
{
    private readonly IMaterialMongoDB _materialRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFavoriteRepository _favoriteRepository;

    public GetMaterialsBySubjectQueryHandler(
        IMaterialMongoDB materialRepository,
        IUserRepository userRepository,
        IFavoriteRepository favoriteRepository)
    {
        _materialRepository = materialRepository;
        _userRepository = userRepository;
        _favoriteRepository = favoriteRepository;
    }

    public async Task<List<MaterialDto>> Handle(GetMaterialsBySubjectQuery request, CancellationToken cancellationToken)
    {
        var materials = await _materialRepository.GetBySubjectId(request.SubjectId);

        if (materials == null)
            return new List<MaterialDto>();

        // Filter by semester if provided
        if (request.Semester.HasValue)
        {
            materials = materials.Where(m => m.Semester == request.Semester.Value).ToList();
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

            result.Add(new MaterialDto
            {
                Id = m.Id.Value,
                SubjectId = m.SubjectId.Value,
                Name = m.Name.Value,
                Year = m.Year.Value,
                Semester = m.Semester,
                Description = m.Description,
                AuthorName = authorName,
                IsFavorite = favoriteMaterialIds.Contains(m.Id.Value),
                MaterialType = m.MaterialType.ToString(),
                Size = m.Size.Size,
                FilePath = m.FilePath.Value,
                UploadedAt = m.UploadedAt
            });
        }

        return result;
    }
}
