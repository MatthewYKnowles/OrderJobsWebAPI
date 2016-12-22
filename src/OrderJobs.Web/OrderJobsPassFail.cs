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
                var testCaseValidation = await GetTestCaseValidation(url, testCase);
                dictionary.Add(count, testCaseValidation);
                count++;
            }
            return dictionary;
        }

        private async Task<TestCaseValidation> GetTestCaseValidation(string url, TestCases testCase)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url + testCase.TestCase);
            string jobOrdering = await response.Content.ReadAsStringAsync();
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder(testCase.TestCase, jobOrdering);
            bool passOrFail = verifyJobOrder.IsValid();
            return new TestCaseValidation(testCase.TestCase, jobOrdering, passOrFail);
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

        protected bool Equals(TestCaseValidation other)
        {
            return string.Equals(testCase, other.testCase) && string.Equals(output, other.output) && string.Equals(result, other.result);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestCaseValidation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (testCase != null ? testCase.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (output != null ? output.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (result != null ? result.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}