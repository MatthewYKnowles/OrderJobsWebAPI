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

        public async Task<TestSuiteResults> GetOrderedJobsPassFailResults(string url)
        {
            string testSuiteResult = "PASS";
            List<TestCasePermutationResults> testCasePermutations = new List<TestCasePermutationResults>();
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
                    testCaseResults.Add(testCaseValidation);
                    count++;
                }
                testCasePermutations.Add(new TestCasePermutationResults(testCase.TestCase, testCaseResult, testCaseResults));
            }
            TestSuiteResults testSuiteResults = new TestSuiteResults(testSuiteResult, testCasePermutations);
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

        public TestSuiteResults(string Result, List<TestCasePermutationResults> Results )
        {
            result = Result;
            results = Results;
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

        public TestCasePermutationResults(string TestCase, string Result, List<TestCaseValidation> Results)
        {
            testCase = TestCase;
            result = Result;
            results = Results;
        }

        public override bool Equals(object obj)
        {
            TestCasePermutationResults testCaseObject = (TestCasePermutationResults)obj;
            return string.Equals(testCase, testCaseObject.testCase) && string.Equals(result, testCaseObject.result) && string.Equals(results, testCaseObject.results);
        }
    }
}