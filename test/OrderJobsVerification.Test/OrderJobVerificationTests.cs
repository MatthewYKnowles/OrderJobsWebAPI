using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OrderJobsVerification.Algorithm;

namespace OrderJobsVerification.Test
{
    [TestFixture]
    public class OrderJobVerificationTests
    {
        [Test]
        public void EmptyJobListTest()
        {
            VerifyJobOrder verifyJobOrder = new VerifyJobOrder("", "");
            Assert.That(verifyJobOrder.IsValid(), Is.EqualTo(false));
        }
    }
}
