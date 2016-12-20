using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderJobs.Web
{
    public class TestCases
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string TestCase { get; set; }
    }
}
