using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderJobs.Algorithm
{
    public class VerifyJobOrder
    {
        private readonly string _unorderedJobs;
        private readonly string _orderedJobsToCheck;
        protected List<Job> _jobs;
        protected bool _hasJobs;

        public VerifyJobOrder(string unorderedJobs, string orderedJobsToCheck)
        {
            _jobs = CreateJobList(unorderedJobs.Split('|'));
            _unorderedJobs = unorderedJobs;
            _orderedJobsToCheck = orderedJobsToCheck;
        }

        protected List<Job> CreateJobList(string[] splitJobs)
        {
            return splitJobs.Select(job =>
            {
                var jobParts1 = job.Split('-');
                var jobName1 = jobParts1.FirstOrDefault();
                var dependency1 = jobParts1.Length > 1 ? jobParts1.ElementAt(1) : "";
                return new Job(jobName1, dependency1);
            }).ToList();
        }

        public bool IsValid()
        {
            return JobsWereNotOrdered() ? NoJobsOrdered() : OrderingOfAllJobsIsValid();
        }

        private bool JobsWereNotOrdered()
        {
            return JobMightDependOnItself() || JobNotAdded() || JobsToOrderMightHaveCircularDependency();
        }

        private bool OrderingOfAllJobsIsValid()
        {
            return NoExtraJobOrJobNotAdded() && CheckToSeeIfJobOrderIsCorrect();
        }

        private bool NoExtraJobOrJobNotAdded()
        {
            return !ExtraJobAdded() && !JobNotAdded();
        }

        private bool NoJobsOrdered()
        {
            if (JobMightDependOnItself())
            {
                return CheckIfJobDependsOnItself();
            }
            else if (JobsToOrderMightHaveCircularDependency())
            {
                return CheckIfCircularDependency();
            }
            else if (NoJobsOrOrderedJobs())
            {
                return true;
            }
            return false;
        }

        private bool JobNotAdded()
        {
            return _jobs.Any(job => !_orderedJobsToCheck.Contains(job.Name));
        }

        private bool ExtraJobAdded()
        {
            string jobsAvailable = _jobs.Aggregate("", (acc, job) => acc + job.Name);
            return _orderedJobsToCheck.Any(job => !jobsAvailable.Contains(job));
        }

        private bool CheckIfCircularDependency()
        {
            var sequenceJobs = new SequenceJobs(_unorderedJobs);
            return sequenceJobs.GetJobSequence() == "Can not resolve circular dependency";
        }

        private bool CheckToSeeIfJobOrderIsCorrect()
        {
            for (var index = 0; index < _orderedJobsToCheck.Length; index++)
            {
                string completedJobs = _orderedJobsToCheck.Substring(0, index);
                Job currentJob = _jobs.Find(job => job.Name == _orderedJobsToCheck[index].ToString());
                if (!completedJobs.Contains(currentJob.Dependency))
                {
                    return false;
                }
            }
            return true;
        }

        private bool NoJobsOrOrderedJobs()
        {
            return _orderedJobsToCheck == "" && _orderedJobsToCheck == "";
        }

        private bool CheckIfJobDependsOnItself()
        {
            return _jobs.Any(job => job.Name == job.Dependency);
        }

        private bool JobMightDependOnItself()
        {
            return _orderedJobsToCheck == "Can not resolve job depending on itself";
        }

        private bool JobsToOrderMightHaveCircularDependency()
        {
            return _orderedJobsToCheck == "Can not resolve circular dependency";
        }
    }
}
