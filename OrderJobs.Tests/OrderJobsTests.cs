using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OrderJobs.Tests
{
    [TestFixture]
    public class OrderJobsTests
    {
        [Test]
        public void NoJobsReturnsEmptyString()
        {
            Algorithm.OrderJobs orderJobs = new Algorithm.OrderJobs("");
            Assert.That(orderJobs.Order(), Is.EqualTo(""));
        }
        [Test]
        public void ReturnJobWithNoDependencies()
        {
            Algorithm.OrderJobs orderJobs = new Algorithm.OrderJobs("a =>");
            Assert.That(orderJobs.Order(), Is.EqualTo("a"));
        }
    }
}
