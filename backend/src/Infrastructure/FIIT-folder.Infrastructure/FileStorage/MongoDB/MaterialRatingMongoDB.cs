using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.FileStorage;

public class MaterialRatingMongoDB : IMaterialRatingRepository
{
    private readonly IMongoCollection<BsonDocument> Collection;

    public MaterialRatingMongoDB(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        Collection = database.GetCollection<BsonDocument>("MaterialRatings");
    }

    public async Task AddAsync(MaterialRating rating)
    {
        var bsonDocument = MapToBsonDocument(rating);
        await Collection.InsertOneAsync(bsonDocument);
    }

    public async Task UpdateAsync(MaterialRating rating)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("ratingId", rating.Id.ToString());
        var update = Builders<BsonDocument>.Update
            .Set("rating", rating.Rating.ToString())
            .Set("updatedAt", rating.UpdatedAt);

        await Collection.UpdateOneAsync(filter, update);
    }

    public async Task<MaterialRating?> GetByUserAndMaterialAsync(UserId userId, MaterialId materialId)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("userId", userId.Value.ToString()),
            Builders<BsonDocument>.Filter.Eq("materialId", materialId.Value.ToString())
        );

        var document = await Collection.Find(filter).FirstOrDefaultAsync();
        return document == null ? null : MapToMaterialRating(document);
    }

    public async Task<List<MaterialRating>> GetByMaterialIdAsync(MaterialId materialId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("materialId", materialId.Value.ToString());
        var documents = await Collection.Find(filter).ToListAsync();
        
        var result = new List<MaterialRating>();
        foreach (var doc in documents)
        {
            result.Add(MapToMaterialRating(doc));
        }
        return result;
    }

    private static MaterialRating MapToMaterialRating(BsonDocument document)
    {
        return new MaterialRating
        {
            Id = Guid.Parse(document["ratingId"].AsString),
            MaterialId = new MaterialId(Guid.Parse(document["materialId"].AsString)),
            UserId = new UserId(Guid.Parse(document["userId"].AsString)),
            Rating = Enum.Parse<RatingType>(document["rating"].AsString),
            CreatedAt = document["createdAt"].ToUniversalTime(),
            UpdatedAt = document["updatedAt"].ToUniversalTime()
        };
    }

    public async Task DeleteAsync(Guid materialId, Guid userId)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("userId", userId.ToString()),
            Builders<BsonDocument>.Filter.Eq("materialId", materialId.ToString())
        );
        await Collection.DeleteOneAsync(filter);
    }

    public async Task<(int likes, int dislikes)> GetRatingCountsAsync(Guid materialId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("materialId", materialId.ToString());
        var documents = await Collection.Find(filter).ToListAsync(cancellationToken);

        int likes = 0;
        int dislikes = 0;

        foreach (var doc in documents)
        {
            if (Enum.TryParse<RatingType>(doc["rating"].AsString, out var rating))
            {
                if (rating == RatingType.Like) likes++;
                else if (rating == RatingType.Dislike) dislikes++;
            }
        }

        return (likes, dislikes);
    }
    
    private static BsonDocument MapToBsonDocument(MaterialRating rating)
    {
        if (rating == null)
            throw new ArgumentNullException(nameof(rating), "Rating не должен быть null");
    
        var bsonDocument = new BsonDocument
        {
            { "_id", rating.Id.ToString() }, // MongoDB needs _id, using generic Id as _id or separately? MaterialMongoDB used "materialId" field and allowed MongoDB to generate _id or used it? MaterialMongoDB used "materialId". I will use "_id" as string guid to be simple, or follow the pattern. MaterialMongoDB used "materialId" and stored it. I'll stick to a custom id field "ratingId" to be safe and consistent with MaterialMongoDB pattern if possible, but _id is standard. Let's look at MaterialMongoDB again. It used "materialId" in the document, and likely let Mongo generate ObjectId for _id or just ignored it.
            // MaterialMongoDB: { "materialId", material.Id.Value.ToString() }
            // It didn't specify "_id".
            // I'll use "ratingId" explicitly.
            { "ratingId", rating.Id.ToString() },
            { "materialId", rating.MaterialId.Value.ToString() },
            { "userId", rating.UserId.Value.ToString() },
            { "rating", rating.Rating.ToString() },
            { "createdAt", rating.CreatedAt },
            { "updatedAt", rating.UpdatedAt }
        };
        
        return bsonDocument;
    }
}
