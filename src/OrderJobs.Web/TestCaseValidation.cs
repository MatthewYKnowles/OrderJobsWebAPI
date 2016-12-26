namespace OrderJobs.Web
{
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
}