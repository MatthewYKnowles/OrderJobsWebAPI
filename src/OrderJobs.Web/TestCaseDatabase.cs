using System.Collections.Generic;
using MongoDB.Driver;

namespace OrderJobs.Web
{
    public class TestCaseDatabase
    {
        static IMongoClient _client = new MongoClient();
        static IMongoDatabase _database = _client.GetDatabase("orderedjobs");
        IMongoCollection<TestCases> _collection = _database.GetCollection<TestCases>("testcases");

        public void InsertTestCase(TestCases testCase)
        {
            _collection.InsertOneAsync(testCase);
        }

        public IEnumerable<TestCases> GetTestCases()
        {
            return _collection.Find(_ => true).ToList();
        }
    }
}