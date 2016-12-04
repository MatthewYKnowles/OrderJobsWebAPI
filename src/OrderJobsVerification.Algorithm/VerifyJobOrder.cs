using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobsVerification.Algorithm
{
    public class VerifyJobOrder
    {
        private string v1;
        private string v2;

        public VerifyJobOrder()
        {
        }

        public VerifyJobOrder(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
