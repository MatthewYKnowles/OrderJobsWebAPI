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

        public async Task<TestCaseSuiteResult> GetTestCaseSuite(string url)
        {
            List<TestCasePermutationResults> testCasePermutations = new List<TestCasePermutationResults>();
            IEnumerable<TestCases> testCaseList = _testCaseDatabase.GetTestCases();
            foreach (TestCases testCase in testCaseList)
            {
                testCasePermutations.Add(await BuildTestCasePermutation(url, testCase.TestCase));
            }
            return new TestCaseSuiteResult(testCasePermutations);
        }

        private async Task<TestCasePermutationResults> BuildTestCasePermutation(string url, string testCase)
        {
            List<TestCaseValidation> testCaseResults = new List<TestCaseValidation>();
            var jobPermutations = new JobPermutations();
            List<string> testCasePermutationsList = jobPermutations.GetPermutations(testCase);
            foreach (string testCasePermutation in testCasePermutationsList)
            {
                testCaseResults.Add(await GetTestCaseValidation(url, testCasePermutation));
            }
            return new TestCasePermutationResults(testCase, testCaseResults);
        }

        private async Task<TestCaseValidation> GetTestCaseValidation(string url, string testCase)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url + testCase);
            string jobOrdering = await response.Content.ReadAsStringAsync();
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder(testCase, jobOrdering);
            bool passOrFail = verifyJobOrder.IsValid();
            return new TestCaseValidation(testCase, passOrFail);
        }
    }
}