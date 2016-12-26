using System.Collections.Generic;
using System.Linq;
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

        public async Task<TestSuiteResults> GetOrderedJobsPassFailResults(string url)
        {
            List<TestCasePermutationResults> testCasePermutations = new List<TestCasePermutationResults>();
            IEnumerable<TestCases> testCaseList = _testCaseDatabase.GetTestCases();
            foreach (TestCases testCase in testCaseList)
            {
                List<TestCaseValidation> testCaseResults = new List<TestCaseValidation>();
                var jobPermutations = new JobPermutations();
                List<string> testCasePermutationsList = jobPermutations.GetPermutations(testCase.TestCase);
                foreach (string testCasePermutation in testCasePermutationsList)
                {
                    var testCaseValidation = await GetTestCaseValidation(url, testCasePermutation);
                    testCaseResults.Add(testCaseValidation);
                }
                testCasePermutations.Add(new TestCasePermutationResults(testCase.TestCase, testCaseResults));
            }
            TestSuiteResults testSuiteResults = new TestSuiteResults(testCasePermutations);
            return testSuiteResults;
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

    public class TestSuiteResults
    {
        public string result { get; }
        public List<TestCasePermutationResults> results { get; }

        public TestSuiteResults(List<TestCasePermutationResults> Results )
        {
            results = Results;
            result = Results.Any(x => x.result == "FAIL") ? "FAIL" : "PASS";
        }
        public override bool Equals(object obj)
        {
            TestSuiteResults testCaseObject = (TestSuiteResults)obj;
            return string.Equals(results, testCaseObject.results) && string.Equals(result, testCaseObject.result);
        }
    }

    public class TestCaseValidation
    {
        public string testCase { get; }
        public string result { get; }

        public TestCaseValidation(string TestCase, bool passOrFail)
        {
            testCase = TestCase;
            result = passOrFail ? "PASS" : "FAIL";
        }

        public override bool Equals(object obj)
        {
            TestCaseValidation testCaseObject = (TestCaseValidation) obj;
            return string.Equals(testCase, testCaseObject.testCase) && string.Equals(result, testCaseObject.result);
        }
    }

    public class TestCasePermutationResults
    {
        public string testCase { get; set; }
        public string result { get; set; }
        public List<TestCaseValidation> results { get; set; }

        public TestCasePermutationResults(string TestCase, List<TestCaseValidation> Results)
        {
            testCase = TestCase;
            results = Results;
            result = Results.Any(x => x.result == "FAIL") ? "FAIL" : "PASS";
        }

        public override bool Equals(object obj)
        {
            TestCasePermutationResults testCaseObject = (TestCasePermutationResults)obj;
            return string.Equals(testCase, testCaseObject.testCase) && string.Equals(result, testCaseObject.result) && string.Equals(results, testCaseObject.results);
        }
    }
}