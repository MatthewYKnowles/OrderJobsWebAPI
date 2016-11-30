using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class SequenceJobs
    {
        private IEnumerable<Job> _jobs = new List<Job>();
        private IEnumerable<Job> _jobsRemaining;
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
            });
        }

        public string GetJobSequence()
        {
            return _hasJobs ? OrderJobs() : "";
        }

        private string OrderJobs()
        {
            _orderedJobs = _jobs.Where(job => job.Dependency == "").Aggregate(_orderedJobs, (acc, job) => acc + job.Name);
            _orderedJobs = _jobs.Where(job => _orderedJobs.Contains(job.Dependency) && job.Dependency != "").ToList().Aggregate(_orderedJobs, (acc, job) => acc + job.Name);
            _jobsRemaining = _jobs.Where(job => !_orderedJobs.Contains(job.Name));
            _orderedJobs = _jobsRemaining.Where(job => _orderedJobs.Contains(job.Dependency) && job.Dependency != "").ToList().Aggregate(_orderedJobs, (acc, job) => acc + job.Name);

            return _orderedJobs;
        }
    }

    public class Job
    {
        public string Name { get; }
        public string Dependency { get; }

        public Job(string name, string dependency)
        {
            Name = name;
            Dependency = dependency;
        }
    }
}
