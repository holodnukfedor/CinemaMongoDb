using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CinemaDAL.Entities.Interfaces
{
    public interface IEntityMongoId
    {
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }
    }
}
