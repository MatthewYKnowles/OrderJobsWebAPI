using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using OrderJobs.Algorithm;

namespace OrderJobs.Web
{
    public class OrderJobsPassFail
    {
        private readonly ITestCaseDatabase _testCaseDatabase;

        public OrderJobsPassFail(ITestCaseDatabase testCaseDatabase)
        {
            _testCaseDatabase = testCaseDatabase;
        }

        public async Task<string> GetOrderedJobsPassFailResults(string url)
        {
            var client = new HttpClient();
            string jobs = "";
            IEnumerable<TestCases> testCaseList = _testCaseDatabase.GetTestCases();
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