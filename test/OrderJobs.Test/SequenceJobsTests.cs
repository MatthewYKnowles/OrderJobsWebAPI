using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Moq;
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

    public class MockTests
    {
        [Test]
        public void FirstTest()
        {
            Mock<ITestCaseDatabase> mockTestCaseDatabase = new Mock<ITestCaseDatabase>();
        }
    }
}
