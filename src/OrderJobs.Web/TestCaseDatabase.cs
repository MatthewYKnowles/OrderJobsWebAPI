using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MongoDB.Driver;
using OrderJobs.Algorithm;

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

        public async Task<string> GetOrderedJobsPassFailResults(string url)
        {
            var client = new HttpClient();
            string jobs = "";
            IEnumerable<TestCases> testCaseList = GetTestCases();
            foreach (var testCase in testCaseList)
            {
                HttpResponseMessage response = await client.GetAsync(url + testCase.TestCase);
                string jobOrdering = await response.Content.ReadAsStringAsync();
                VerifyJobOrder verifyJobOrder = new VerifyJobOrder(testCase.TestCase, jobOrdering);
                bool passOrFail = verifyJobOrder.IsValid();
                jobs += testCase.TestCase + " -> " + jobOrdering + " : " + passOrFail + "\n";
            }
            return jobs;
        }
    }
}