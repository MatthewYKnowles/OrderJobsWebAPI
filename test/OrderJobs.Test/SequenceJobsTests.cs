using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OrderJobs.Algorithm;

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
            Assert.Throws<ArgumentOutOfRangeException>(() => sequenceJobs.GetJobSequence());
        }
        [Test]
        public void CircularDependency()
        {
            var sequenceJobs = new SequenceJobs("a-b|b-c|c-d|d-a|e-f|f-e");
            Assert.Throws<ArgumentOutOfRangeException>(() => sequenceJobs.GetJobSequence());
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
    }
}
