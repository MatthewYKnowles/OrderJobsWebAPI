using System.Collections.Generic;
using System.Linq;

namespace OrderJobs.Algorithm
{
    public class VerifyJobOrder
    {
        private readonly string _orderedJobsToCheck;
        private bool _validOrdering;
        protected List<Job> _jobs;
        protected bool _hasJobs;

        public VerifyJobOrder(string unorderedJobs, string orderedJobsToCheck)
        {
            _jobs = CreateJobList(unorderedJobs.Split('|'));
            _orderedJobsToCheck = orderedJobsToCheck;
        }

        protected List<Job> CreateJobList(string[] splitJobs)
        {
            List<Job> jobs = splitJobs.Select(job =>
            {
                var jobParts = job.Split('-');
                var jobName = jobParts.FirstOrDefault();
                var dependency = jobParts.Length > 1 ? jobParts.ElementAt(1) : "";
                return new Job(jobName, dependency);
            }).ToList();
            return jobs;
        }

        public bool IsValid()
        {
            if (jobMightDependOnItself())
            {
                checkIfJobDependsOnItself();
            }
            else if (noJobsOrOrderedJobs())
            {
                _validOrdering = true;
            }
            else
            {
                checkToSeeIfJobOrderIsCorrect();
            }
            return _validOrdering;
        }

        private void checkToSeeIfJobOrderIsCorrect()
        {
            _validOrdering = true;
            for (var index = 0; index < _orderedJobsToCheck.Length; index++)
            {
                string alreadyAddedJobs = _orderedJobsToCheck.Substring(0, index);
                Job currentJob = _jobs.Find(job => job.Name == _orderedJobsToCheck[index].ToString());
                if (currentJob == null)
                {
                    _validOrdering = false;
                }
                else if (!alreadyAddedJobs.Contains(currentJob.Dependency))
                {
                    _validOrdering = false;
                }
            }
        }

        private bool noJobsOrOrderedJobs()
        {
            return _orderedJobsToCheck == "" && _orderedJobsToCheck == "";
        }

        private void checkIfJobDependsOnItself()
        {
            _validOrdering = false;
            foreach (Job job in _jobs)
            {
                if (job.Name == job.Dependency)
                {
                    _validOrdering = true;
                }
            }
        }

        private bool jobMightDependOnItself()
        {
            return _orderedJobsToCheck == "Can not resolve job depending on itself";
        }
    }
}
