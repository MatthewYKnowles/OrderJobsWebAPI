using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class SequenceJobs
    {
        private List<Job> _jobs = new List<Job>();
        private readonly bool _hasJobs;
        private string _orderedJobs = "";

        public SequenceJobs(string jobs)
        {
            _hasJobs = jobs != "";
            CreateJobList(jobs.Split('|'));
        }

        private void CreateJobList(string[] splitJobs)
        {
            _jobs = splitJobs.Select(job =>
            {
                var jobParts = job.Split('-');
                var jobName = jobParts.FirstOrDefault();
                var dependency = jobParts.Length > 1 ? jobParts.ElementAt(1) : "";
                return new Job(jobName, dependency);
            }).ToList();
        }

        public string GetJobSequence()
        {
            return _hasJobs ? OrderJobs() : "";
        }

        private string OrderJobs()
        {
            AddJobsWithNoDependencies();
            CheckForJobDependingOnSelf();
            AddJobsWithDependencies();
            return _orderedJobs;
        }

        private void CheckForJobDependingOnSelf()
        {
            foreach (Job job in _jobs)
            {
                if (job.Name == job.Dependency)
                {
                    throw new ArgumentOutOfRangeException();
                }
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
                    throw new ArgumentOutOfRangeException();
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
}
