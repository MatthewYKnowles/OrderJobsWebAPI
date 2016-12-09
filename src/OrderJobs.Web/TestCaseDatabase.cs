using System.Collections.Generic;
using MongoDB.Driver;

namespace OrderJobs.Web
{
    public class TestCaseDatabase
    {
        private static readonly IMongoClient Client = new MongoClient();
        private static readonly IMongoDatabase Database = Client.GetDatabase("orderedjobs");
        private readonly IMongoCollection<TestCases> _collection = Database.GetCollection<TestCases>("testcases");

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