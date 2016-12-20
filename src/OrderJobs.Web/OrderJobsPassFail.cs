using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using OrderJobs.Algorithm;

namespace OrderJobs.Web
{
    public class OrderJobsPassFail
    {
        private readonly ITestCaseDatabase _testCaseDatabase;
        private readonly IHttpClient _httpClient;

        public OrderJobsPassFail(ITestCaseDatabase testCaseDatabase, IHttpClient httpClient)
        {
            _testCaseDatabase = testCaseDatabase;
            _httpClient = httpClient;
        }

        public async Task<string> GetOrderedJobsPassFailResults(string url)
        {
            string jobs = "";
            IEnumerable<TestCases> testCaseList = _testCaseDatabase.GetTestCases();
            foreach (var testCase in testCaseList)
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url + testCase.TestCase);
                string jobOrdering = await response.Content.ReadAsStringAsync();
                VerifyJobOrder verifyJobOrder = new VerifyJobOrder(testCase.TestCase, jobOrdering);
                bool passOrFail = verifyJobOrder.IsValid();
                jobs += testCase.TestCase + " -> " + jobOrdering + " : " + passOrFail + "\n";
            }
            return jobs;
        }
    }
}