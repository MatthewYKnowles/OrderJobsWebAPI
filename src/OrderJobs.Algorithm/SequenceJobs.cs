using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class SequenceJobs
    {
        private IEnumerable<Job> _jobs = new List<Job>();
        private readonly bool _hasJobs;

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
            string jobs = "";
            foreach (Job job in _jobs)
            {
                if (job.Dependency == "")
                {
                    jobs += job.Name;
                }
            }
            foreach (Job job in _jobs)
            {
                if (job.Dependency.Length > 0)
                {
                    jobs += job.Name;
                }
            }

            return jobs;
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
