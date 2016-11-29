using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class SequenceJobs
    {
        private List<Job> _jobs = new List<Job>();
        private readonly string[] _splitJobs;
        private readonly bool _hasJobs;

        public SequenceJobs(string jobs)
        {
            _hasJobs = jobs != "";
            _splitJobs = jobs.Split('|');
            foreach (string job in _splitJobs)
            {
                string jobName = "";
                string jobDependency = "";
                if (job.Length > 0)
                {
                    jobName = job[0].ToString();
                }
                if (job.Length > 2)
                {
                    jobDependency = job[2].ToString();
                }
                _jobs.Add(new Job(jobName, jobDependency));
            }
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
                jobs += job.Name;
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
