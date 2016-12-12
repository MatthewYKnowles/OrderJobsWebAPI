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
                string jobsAvailable = _jobs.Aggregate("", (acc, job) => acc + job.Name);
                if (_orderedJobsToCheck.Any(job => !jobsAvailable.Contains(job)))
                {
                    _validOrderingOfJobs = false;
                }
                else
                {
                    CheckToSeeIfAJobWasNotAdded();
                }
            }
            return _validOrderingOfJobs;
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
            if (_jobs.Any(job => !_orderedJobsToCheck.Contains(job.Name)))
            {
                _validOrderingOfJobs = false;
            };
        }

        private void CheckToSeeIfJobOrderIsCorrect()
        {
            for (var index = 0; index < _orderedJobsToCheck.Length; index++)
            {
                string completedJobs = _orderedJobsToCheck.Substring(0, index);
                if (JobDoesNotExistOrDependencyIsNotAlreadyAdded(index, completedJobs))
                {
                    _validOrderingOfJobs = false;
                }
            }
        }

        private bool JobDoesNotExistOrDependencyIsNotAlreadyAdded(int index, string alreadyAddedJobs)
        {
            var currentJob = _jobs.Find(job =>
            {
                var jobNameToCheck = _orderedJobsToCheck[index].ToString();
                return job.Name == jobNameToCheck;
            });
            return currentJob == null || !alreadyAddedJobs.Contains(currentJob.Dependency);
        }

        private bool NoJobsOrOrderedJobs()
        {
            return _orderedJobsToCheck == "" && _orderedJobsToCheck == "";
        }

        private void CheckIfJobDependsOnItself()
        {
            _validOrderingOfJobs = _jobs.Any(job => job.Name == job.Dependency);
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
