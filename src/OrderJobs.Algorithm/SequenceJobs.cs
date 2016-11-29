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
            _jobs.Add(new Job("a", "b"));
        }

        public string GetJobSequence()
        {
            return _hasJobs ? OrderJobs() : "";
        }

        private string OrderJobs()
        {
            string jobs = "";
            foreach (string job in _splitJobs)
            {
                jobs += job[0];
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
