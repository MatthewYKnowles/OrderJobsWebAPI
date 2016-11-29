using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OrderJobs.Algorithm;

namespace OrderJobs.Tests
{
    [TestFixture]
    public class OrderJobsTests
    {
        [Test]
        public void NoJobsReturnsEmptyString()
        {
            var orderJobs = new JobOrderer("");
            Assert.That(orderJobs.Order(), Is.EqualTo(""));
        }
        [Test]
        public void ReturnJobWithNoDependencies()
        {
            var orderJobs = new JobOrderer("a =>");
            Assert.That(orderJobs.Order(), Is.EqualTo("a"));
        }
    }
}
