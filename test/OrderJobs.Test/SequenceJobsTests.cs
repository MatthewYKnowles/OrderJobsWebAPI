using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Serializers;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using OrderJobs.Algorithm;
using OrderJobs.Web;

namespace OrderJobs.Test
{
    [TestFixture]
    public class SequenceJobsTests
    {
        [Test]
        public void EmptyStringTest()
        {
            var sequenceJobs = new SequenceJobs("");
            Assert.That(sequenceJobs.GetJobSequence(), Is.EqualTo(""));
        }
        [Test]
        public void OneJobNoDependency()
        {
            var sequenceJobs = new SequenceJobs("a-");
            Assert.That(sequenceJobs.GetJobSequence(), Is.EqualTo("a"));
        }
        [Test]
        public void TwoJobsNoDependencies()
        {
            var sequenceJobs = new SequenceJobs("a-|b-");
            Assert.That(sequenceJobs.GetJobSequence(), Is.EqualTo("ab"));
        }
        [Test]
        public void TwoJobsOneDependency()
        {
            var sequenceJobs = new SequenceJobs("a-b|b-");
            Assert.That(sequenceJobs.GetJobSequence(), Is.EqualTo("ba"));
        }
        [Test]
        public void ThreeJobsWithSomeDependencies()
        {
            var sequenceJobs = new SequenceJobs("a-b|b-c|c-");
            Assert.That(sequenceJobs.GetJobSequence(), Is.EqualTo("cba"));
        }
        [Test]
        public void SixJobsWithLotsOfDependencies()
        {
            var sequenceJobs = new SequenceJobs("a-b|b-c|c-|d-a|e-");
            Assert.That(sequenceJobs.GetJobSequence(), Is.EqualTo("cebad"));
        }
        [Test]
        public void JobDependsOnSelf()
        {
            var sequenceJobs = new SequenceJobs("a-b|b-c|c-c|d-a|e-");
            Assert.Throws<SelfReferenceException>(() => sequenceJobs.OrderJobs());
        }
        [Test]
        public void CircularDependency()
        {
            var sequenceJobs = new SequenceJobs("a-b|b-c|c-d|d-a|e-f|f-e");
            Assert.Throws<CircularDependencyException>(() => sequenceJobs.OrderJobs());
        }
    }

    [TestFixture]
    public class OrderedJobsVerificationTests
    {
        [Test]
        public void EmptyJobListTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("", "");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(true));
        }
        [Test]
        public void WrongJobReturnedTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-", "b");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(false));
        }
        [Test]
        public void TwoJobsNoDependenciesTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-|b-", "ab");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(true));
        }
        [Test]
        public void TwoJobsSwitchedOrderTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-|b-", "ba");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(true));
        }
        [Test]
        public void JobNotInListTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-|b-", "bac");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(false));
        }
        [Test]
        public void ThreeJobsOneDependencyTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-b|b-|c-", "bca");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(true));
        }
        [Test]
        public void ThreeJobsOneDependencyDifferentOrderTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-b|b-|c-", "bac");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(true));
        }
        [Test]
        public void ThreeJobsOneDependencyWrongOrderTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-b|b-|c-", "abc");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(false));
        }
        [Test]
        public void FiveJobsFourDependenciesTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-b|b-c|c-|d-b|e-d", "cbade");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(true));
        }
        [Test]
        public void FiveJobsFourDependenciesWithIncorrectOrderTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-b|b-c|c-|d-b|e-d", "cbaed");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(false));
        }
        [Test]
        public void ExtraJobNotAddedToStringTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-|b-|c-|d-|e-|f-", "cbaed");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(false));
        }
        [Test]
        public void CheckForJobDependingOnItselfIsCorrectTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-b|b-c|c-c", "Can not resolve job depending on itself");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(true));
        }
        [Test]
        public void CheckForJobDependingOnItselfNotCorrectTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-b|b-c|c-", "Can not resolve job depending on itself");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(false));
        }
        [Test]
        public void CheckIfCircularDependencyIsCorrectTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-b|b-c|c-a", "Can not resolve circular dependency");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(true));
        }
        [Test]
        public void CheckIfCircularDependencyNotCorrectTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("a-b|b-c|c-", "Can not resolve circular dependency");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(false));
        }
    }

    [TestFixture]
    public class PermutationTests
    {
        [Test]
        public void NoJobPermutations()
        {
            var jobPermutations = new JobPermutations();
            List<string> permutations = new List<string> { "" };
            Assert.That(jobPermutations.GetPermutations(""), Is.EqualTo(permutations));
        }
        [Test]
        public void OneJobPermutations()
        {
            var jobPermutations = new JobPermutations();
            List<string> permutations = new List<string> { "a-" };
            Assert.That(jobPermutations.GetPermutations("a-"), Is.EqualTo(permutations));
        }
        [Test]
        public void TwoJobPermutations()
        {
            var jobPermutations = new JobPermutations();
            List<string> permutations = new List<string> { "a-|b-a", "b-a|a-" };
            Assert.That(jobPermutations.GetPermutations("a-|b-a"), Is.EqualTo(permutations));
        }
        [Test]
        public void ThreeJobPermutations()
        {
            var jobPermutations = new JobPermutations();
            List<string> permutations = new List<string> { "a-|b-a|c-a", "b-a|a-|c-a", "c-a|a-|b-a", "a-|c-a|b-a", "b-a|c-a|a-", "c-a|b-a|a-" };
            Assert.That(jobPermutations.GetPermutations("a-|b-a|c-a"), Is.EqualTo(permutations));
        }
    }

    public class SampleTestCase
    {
        public string TestCase { get; set; }
    }

    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void CheckTwoJobs()
        {
            var httpClient = new HttpClient();
            Task<HttpResponseMessage> response = httpClient.GetAsync("http://localhost:55163/api/values/a-b%7Cb-");
            response.Wait();
            Assert.That(response.Result.Content.ReadAsStringAsync().Result, Is.EqualTo("ba"));
        }
        [Test]
        public void ClearDatabaseAndCheckTestCase()
        {
            var httpClient = new HttpClient();
            var deleteResponse = httpClient.DeleteAsync("http://localhost:55163/api/test/all");
            deleteResponse.Wait();
            var testCaseJson = JsonConvert.SerializeObject(new SampleTestCase {TestCase = "a-|b-"});
            var testCaseWithJsonHeaders = new StringContent(testCaseJson, Encoding.UTF8, "application/json");
            var postResponse = httpClient.PostAsync("http://localhost:55163/api/test", testCaseWithJsonHeaders);
            postResponse.Wait();
            Task<HttpResponseMessage> testCaseResultsFromApi = httpClient.GetAsync("http://localhost:55163/api/test?url=http://localhost:55163/api/values/");
            testCaseResultsFromApi.Wait();
            var testCaseResultAsJson = testCaseResultsFromApi.Result.Content.ReadAsStringAsync().Result;
            var expectedResult = new TestCaseSuiteResult(new List<TestCasePermutationResults>
            {
                new TestCasePermutationResults("a-|b-",
                    new List<TestCaseValidation>
                    {
                        new TestCaseValidation("a-|b-", true),
                        new TestCaseValidation("b-|a-", true)
                    })
            });
            var expectedResultAsJson = JsonConvert.SerializeObject(expectedResult);
            Assert.That(testCaseResultAsJson, Is.EqualTo(expectedResultAsJson));

        }
    }

    [TestFixture]
    public class MockTests
    {
        [Test]
        public void SendBothPermutationsThroughHttpClient()
        {
            Mock<ITestCaseDatabase> mockTestCaseDatabase = new Mock<ITestCaseDatabase>();
            Mock<IHttpClient> mockHttpClient = new Mock<IHttpClient>();
            var orderJobsPassFail = new OrderJobsPassFail(mockTestCaseDatabase.Object, mockHttpClient.Object);
            mockTestCaseDatabase.Setup(x => x.GetTestCases()).Returns(() => new List<TestCases>
                {new TestCases {TestCase = "a-|b-"}});
            mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                Content = new FormUrlEncodedContent(new[]
                    {new KeyValuePair<string, string>("test", "test")})
            });

            var testCaseValidations = orderJobsPassFail.GetTestCaseSuite("http://test/").Result;

            mockHttpClient.Verify(x => x.GetAsync("http://test/a-|b-"));
            mockHttpClient.Verify(x => x.GetAsync("http://test/b-|a-"));
        }

        [Test]
        public void BothPermutationsOfATestCase()
        {
            Mock<ITestCaseDatabase> mockTestCaseDatabase = new Mock<ITestCaseDatabase>();
            Mock<IHttpClient> mockHttpClient = new Mock<IHttpClient>();
            var orderJobsPassFail = new OrderJobsPassFail(mockTestCaseDatabase.Object, mockHttpClient.Object);
            mockTestCaseDatabase.Setup(x => x.GetTestCases()).Returns(() => new List<TestCases>
                {new TestCases {TestCase = "a-|b-"}});
            mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                Content = new StringContent("ab")
            });

            var testCaseValidations = orderJobsPassFail.GetTestCaseSuite("http://test/").Result;
            TestCaseValidation testCaseOne = new TestCaseValidation("a-|b-", true);
            TestCaseValidation testCaseTwo = new TestCaseValidation("b-|a-", true);
            List<TestCaseValidation> testCaseValidationList = new List<TestCaseValidation>() { testCaseOne, testCaseTwo };
            List<TestCasePermutationResults> testCasePermutations = new List<TestCasePermutationResults>();
            TestCasePermutationResults testCasePermutationResults = new TestCasePermutationResults("a-|b-",
                testCaseValidationList);
            testCasePermutations.Add(testCasePermutationResults);
            TestCaseSuiteResult testCaseSuiteResult = new TestCaseSuiteResult(testCasePermutations);

            Assert.That(testCaseValidations.results[0].results[0], Is.EqualTo(testCaseSuiteResult.results[0].results[0]));
            Assert.That(testCaseValidations.results[0].results[1], Is.EqualTo(testCaseSuiteResult.results[0].results[1]));
        }
        [Test]
        public void FailuresMoveUpToTestSuiteLevel()
        {
            Mock<ITestCaseDatabase> mockTestCaseDatabase = new Mock<ITestCaseDatabase>();
            Mock<IHttpClient> mockHttpClient = new Mock<IHttpClient>();
            var orderJobsPassFail = new OrderJobsPassFail(mockTestCaseDatabase.Object, mockHttpClient.Object);
            mockTestCaseDatabase.Setup(x => x.GetTestCases()).Returns(() => new List<TestCases>
                {new TestCases {TestCase = "a-b|b-"}});
            mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                Content = new StringContent("ab")
            });

            var testCaseValidations = orderJobsPassFail.GetTestCaseSuite("http://test/").Result;
            TestCaseValidation testCaseOne = new TestCaseValidation("a-b|b-", false);
            TestCaseValidation testCaseTwo = new TestCaseValidation("b-|a-b", false);
            List<TestCaseValidation> testCaseValidationList = new List<TestCaseValidation>() { testCaseOne, testCaseTwo };
            List<TestCasePermutationResults> testCasePermutations = new List<TestCasePermutationResults>();
            TestCasePermutationResults testCasePermutationResults = new TestCasePermutationResults("a-|b-",
                testCaseValidationList);
            testCasePermutations.Add(testCasePermutationResults);
            TestCaseSuiteResult testCaseSuiteResult = new TestCaseSuiteResult(testCasePermutations);

            Assert.That(testCaseValidations.results[0].result, Is.EqualTo(testCaseSuiteResult.results[0].result));
            Assert.That(testCaseValidations.result, Is.EqualTo(testCaseSuiteResult.result));
        }
        [Test]
        public void FinalObjectsAreTheSame()
        {
            Mock<ITestCaseDatabase> mockTestCaseDatabase = new Mock<ITestCaseDatabase>();
            Mock<IHttpClient> mockHttpClient = new Mock<IHttpClient>();
            var orderJobsPassFail = new OrderJobsPassFail(mockTestCaseDatabase.Object, mockHttpClient.Object);
            mockTestCaseDatabase.Setup(x => x.GetTestCases()).Returns(() => new List<TestCases>
                {new TestCases {TestCase = "a-b|b-"}});
            mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                Content = new StringContent("ab")
            });

            var testCaseValidations = orderJobsPassFail.GetTestCaseSuite("http://test/").Result;
            TestCaseValidation testCaseOne = new TestCaseValidation("a-b|b-", false);
            TestCaseValidation testCaseTwo = new TestCaseValidation("b-|a-b", false);
            List<TestCaseValidation> testCaseValidationList = new List<TestCaseValidation>() { testCaseOne, testCaseTwo };
            List<TestCasePermutationResults> testCasePermutations = new List<TestCasePermutationResults>();
            TestCasePermutationResults testCasePermutationResults = new TestCasePermutationResults("a-|b-",
                testCaseValidationList);
            testCasePermutations.Add(testCasePermutationResults);
            TestCaseSuiteResult testCaseSuiteResult = new TestCaseSuiteResult(testCasePermutations);

            Assert.That(testCaseValidations.results[0].results, Is.EqualTo(testCaseSuiteResult.results[0].results));
        }
        [Test]
        public void TestCasesAreTheSame()
        {
            Mock<ITestCaseDatabase> mockTestCaseDatabase = new Mock<ITestCaseDatabase>();
            Mock<IHttpClient> mockHttpClient = new Mock<IHttpClient>();
            var orderJobsPassFail = new OrderJobsPassFail(mockTestCaseDatabase.Object, mockHttpClient.Object);
            mockTestCaseDatabase.Setup(x => x.GetTestCases()).Returns(() => new List<TestCases>
                {new TestCases {TestCase = "a-b|b-"}});
            mockHttpClient.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                Content = new StringContent("ab")
            });

            var testCaseValidations = orderJobsPassFail.GetTestCaseSuite("http://test/").Result;
            var testCaseSuiteResult = new TestCaseSuiteResult(new List<TestCasePermutationResults>
            {
                new TestCasePermutationResults("a-b|b-",
                    new List<TestCaseValidation>
                    {
                        new TestCaseValidation("a-b|b-", false),
                        new TestCaseValidation("b-|a-b", false)
                    })
            });

            Assert.That(testCaseValidations.results[0].testCase, Is.EqualTo(testCaseSuiteResult.results[0].testCase));
        }
    }
}
