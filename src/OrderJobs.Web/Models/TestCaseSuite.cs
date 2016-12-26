using System.Collections.Generic;
using System.Linq;

namespace OrderJobs.Web
{
    public class TestCaseSuite
    {
        public string result { get; }
        public List<TestCasePermutationResults> results { get; }

        public TestCaseSuite(List<TestCasePermutationResults> Results )
        {
            results = Results;
            result = Results.Any(x => x.result == "FAIL") ? "FAIL" : "PASS";
        }
    }
}