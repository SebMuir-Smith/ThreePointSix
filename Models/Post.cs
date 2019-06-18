using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ThreePointSix.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public BsonArray Dates { get; set; }

        public BsonString Message { get; set; }

        public BsonArray Authors { get; set; }
    }
}