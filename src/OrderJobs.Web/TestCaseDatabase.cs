using System.Collections.Generic;
using MongoDB.Driver;

namespace OrderJobs.Web
{
    public interface ITestCaseDatabase
    {
        void InsertTestCase(TestCases testCase);
        IEnumerable<TestCases> GetTestCases();
    }

    public class TestCaseDatabase : ITestCaseDatabase
    {
        private readonly IMongoCollection<TestCases> _collection;

        public TestCaseDatabase(IMongoClient client)
        {
            _collection = client
                .GetDatabase("orderedjobs")
                .GetCollection<TestCases>("testcases");
        }

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