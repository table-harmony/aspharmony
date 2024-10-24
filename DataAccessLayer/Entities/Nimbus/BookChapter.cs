using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace DataAccessLayer.Entities.Nimbus {
    public class BookChapter {

        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId? Id { get; set; }

        public int BookId { get; set; } 

        public int Index { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
