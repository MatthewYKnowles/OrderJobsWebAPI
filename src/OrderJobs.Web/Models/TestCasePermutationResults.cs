using System.Collections.Generic;
using System.Linq;

namespace OrderJobs.Web
{
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
    }
}