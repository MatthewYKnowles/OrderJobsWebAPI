using System.Collections.Generic;
using System.Linq;

namespace OrderJobs.Web
{
    public class TestCaseSuiteResult
    {
        public string result { get; }
        public List<TestCasePermutationResults> results { get; }

        public TestCaseSuiteResult(List<TestCasePermutationResults> Results )
        {
            results = Results;
            result = Results.Any(x => x.result == "FAIL") ? "FAIL" : "PASS";
        }

        protected bool Equals(TestCaseSuiteResult other)
        {
            return string.Equals(result, other.result) && Equals(results, other.results);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestCaseSuiteResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((result != null ? result.GetHashCode() : 0) * 397) ^ (results != null ? results.GetHashCode() : 0);
            }
        }
    }
}