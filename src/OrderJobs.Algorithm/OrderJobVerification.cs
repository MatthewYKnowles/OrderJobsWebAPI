using System.Collections.Generic;
using System.Linq;

namespace OrderJobs.Algorithm
{
    public class VerifyJobOrder : OrderingJobs
    {
        private readonly string _orderedJobsToCheck;
        private List<Job> _jobs;

        public VerifyJobOrder(string unorderedJobs, string orderedJobsToCheck)
        {
            _jobs = CreateJobList(unorderedJobs.Split('|'));
            _orderedJobsToCheck = orderedJobsToCheck;
        }

        public bool IsValid()
        {
            if (_orderedJobsToCheck != _jobs[0].Name)
            {
                return false;
            }
            return true;
        }
    }
}
