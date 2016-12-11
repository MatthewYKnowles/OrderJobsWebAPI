using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderJobs.Algorithm
{
    public class VerifyJobOrder
    {
        private readonly string _unorderedJobs;
        private readonly string _orderedJobsToCheck;
        private bool _validOrderingOfJobs;
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
            if (JobMightDependOnItself())
            {
                CheckIfJobDependsOnItself();
            }
            else if (JobsToOrderMightHaveCircularDependency())
            {
                CheckIfCircularDependency();
            }
            else if (NoJobsOrOrderedJobs())
            {
                _validOrderingOfJobs = true;
            }
            else
            {
                _validOrderingOfJobs = true;
                CheckToSeeIfJobOrderIsCorrect();
                CheckToSeeIfAJobWasNotAdded();
            }
            return _validOrderingOfJobs;
        }

        private bool JobsToOrderMightHaveCircularDependency()
        {
            return _orderedJobsToCheck == "Can not resolve circular dependency";
        }

        private void CheckIfCircularDependency()
        {
            _validOrderingOfJobs = false;
            var sequenceJobs = new SequenceJobs(_unorderedJobs);
            if (sequenceJobs.GetJobSequence() == "Can not resolve circular dependency")
            {
                _validOrderingOfJobs = true;
            }
        }

        private void CheckToSeeIfAJobWasNotAdded()
        {
            foreach (Job job in _jobs)
            {
                if (!_orderedJobsToCheck.Contains(job.Name))
                {
                    _validOrderingOfJobs = false;
                }
            }
        }

        private void CheckToSeeIfJobOrderIsCorrect()
        {
            for (var index = 0; index < _orderedJobsToCheck.Length; index++)
            {
                string alreadyAddedJobs = _orderedJobsToCheck.Substring(0, index);
                Job currentJob = _jobs.Find(job => job.Name == _orderedJobsToCheck[index].ToString());
                if (currentJob == null || !alreadyAddedJobs.Contains(currentJob.Dependency))
                {
                    _validOrderingOfJobs = false;
                }
            }
        }

        private bool NoJobsOrOrderedJobs()
        {
            return _orderedJobsToCheck == "" && _orderedJobsToCheck == "";
        }

        private void CheckIfJobDependsOnItself()
        {
            _validOrderingOfJobs = false;
            foreach (Job job in _jobs)
            {
                if (job.Name == job.Dependency)
                {
                    _validOrderingOfJobs = true;
                }
            }
        }

        private bool JobMightDependOnItself()
        {
            return _orderedJobsToCheck == "Can not resolve job depending on itself";
        }
    }
}
