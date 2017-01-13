using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class SequenceJobs
    {
        private List<Job> _jobs;
        private string _orderedJobs = "";
        protected bool _hasJobs;

        public SequenceJobs(string jobs)
        {
            _hasJobs = jobs != "";
            _jobs = CreateJobList(jobs.Split('|'));
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

        public string GetJobSequence()
        {
            if (!_hasJobs)
            {
                return "";
            }
            try
            {
                return OrderJobs();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string OrderJobs()
        {
            AddJobsWithNoDependencies();
            CheckForJobDependingOnSelf();
            AddJobsWithDependencies();
            return _orderedJobs;
        }

        private void CheckForJobDependingOnSelf()
        {
            if (_jobs.Any(job => job.Name == job.Dependency))
            {
                throw new SelfReferenceException();
            }
        }

        private void AddJobsWithDependencies()
        {
            int jobsLeft = _jobs.Count;
            while (_jobs.Count > 0)
            {
                jobsLeft = _jobs.Count;
                CollectUnaddedJobs();
                AddJobsWithDependenciesMet();
                if (jobsLeft == _jobs.Count)
                {
                    throw new CircularDependencyException();
                }
            }
        }

        private void AddJobsWithDependenciesMet()
        {
            _orderedJobs = _jobs.Where(job => _orderedJobs.Contains(job.Dependency))
                    .Aggregate(_orderedJobs, (acc, job) => acc + job.Name);
        }

        private void CollectUnaddedJobs()
        {
            _jobs = _jobs.Where(job => !_orderedJobs.Contains(job.Name)).ToList();
        }

        private void AddJobsWithNoDependencies()
        {
            _orderedJobs = _jobs.Where(job => job.Dependency == "")
                .Aggregate(_orderedJobs, (acc, job) => acc + job.Name);
        }
    }

    public class SelfReferenceException : Exception
    {
        public override string Message => "Can not resolve job depending on itself";
    }

    public class CircularDependencyException : Exception
    {
        public override string Message => "Can not resolve circular dependency";
    }
}
