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
            var sequenceJobs = new SequenceJobs();
            Assert.That(sequenceJobs.Sequence(""), Is.EqualTo(""));
        }

        [Test]
        public void OneJobNoDependency()
        {
            var sequenceJobs = new SequenceJobs();
            Assert.That(sequenceJobs.Sequence("a-"), Is.EqualTo("a"));
        }
    }
}
