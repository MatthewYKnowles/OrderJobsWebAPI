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

        public async Task<Dictionary<int, TestCaseValidation>> GetOrderedJobsPassFailResults(string url)
        {
            int count = 1;
            Dictionary<int, TestCaseValidation> dictionary = new Dictionary<int, TestCaseValidation>();
            IEnumerable<TestCases> testCaseList = _testCaseDatabase.GetTestCases();
            foreach (var testCase in testCaseList)
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url + testCase.TestCase);
                string jobOrdering = await response.Content.ReadAsStringAsync();
                VerifyJobOrder verifyJobOrder = new VerifyJobOrder(testCase.TestCase, jobOrdering);
                bool passOrFail = verifyJobOrder.IsValid();
                TestCaseValidation testCaseValidation = new TestCaseValidation(testCase.TestCase, jobOrdering, passOrFail);
                dictionary.Add(count, testCaseValidation);
                count++;
            }
            return dictionary;
        }
    }

    public class TestCaseValidation
    {
        public string testCase { get; }
        public string output { get; }
        public string result { get; }

        public TestCaseValidation(string TestCase, string jobOrdering, bool passOrFail)
        {
            testCase = TestCase;
            output = jobOrdering;
            result = passOrFail ? "PASS" : "FAIL";
        }
    }
}