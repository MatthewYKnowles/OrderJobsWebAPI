using System.Collections.Generic;
using System.Linq;

namespace OrderJobs.Algorithm
{
    public class VerifyJobOrder : OrderingJobs
    {
        private readonly string _orderedJobsToCheck;

        public VerifyJobOrder(string unorderedJobs, string orderedJobsToCheck)
        {
            _jobs = CreateJobList(unorderedJobs.Split('|'));
            _orderedJobsToCheck = orderedJobsToCheck;
        }

        public bool IsValid()
        {
            if (_orderedJobsToCheck == _jobs[0].Name) return true;
            return false;
        }
    }
}
