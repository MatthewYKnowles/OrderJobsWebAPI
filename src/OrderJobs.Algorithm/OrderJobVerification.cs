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
            bool validOrdering = true;
            if (_orderedJobsToCheck == "" && _orderedJobsToCheck == "")
            {
                return true;
            }
            for (var index = 0; index < _orderedJobsToCheck.Length; index++)
            {
                Job currentJob = _jobs.Find(job => job.Name == _orderedJobsToCheck[index].ToString());
                if (currentJob == null)
                {
                    validOrdering = false;
                }
            }
            return validOrdering;
        }
    }
}
