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
            string testSuiteResult = "PASS";
            int count = 1;
            Dictionary<int, TestCaseValidation> dictionary = new Dictionary<int, TestCaseValidation>();
            IEnumerable<TestCases> testCaseList = _testCaseDatabase.GetTestCases();
            foreach (TestCases testCase in testCaseList)
            {
                string testCaseResult = "PASS";
                List<TestCaseValidation> testCaseResults = new List<TestCaseValidation>();
                var jobPermutations = new JobPermutations();
                List<string> testCasePermutationsList = jobPermutations.GetPermutations(testCase.TestCase);
                foreach (string testCasePermutation in testCasePermutationsList)
                {
                    var testCaseValidation = await GetTestCaseValidation(url, testCasePermutation);
                    dictionary.Add(count, testCaseValidation);
                    count++;
                }
            }
            return dictionary;
        }

        private async Task<TestCaseValidation> GetTestCaseValidation(string url, string testCase)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url + testCase);
            string jobOrdering = await response.Content.ReadAsStringAsync();
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder(testCase, jobOrdering);
            bool passOrFail = verifyJobOrder.IsValid();
            return new TestCaseValidation(testCase, jobOrdering, passOrFail);
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

        public override bool Equals(object obj)
        {
            TestCaseValidation testCaseObject = (TestCaseValidation) obj;
            return string.Equals(testCase, testCaseObject.testCase) && string.Equals(output, testCaseObject.output) && string.Equals(result, testCaseObject.result);
        }
    }
}