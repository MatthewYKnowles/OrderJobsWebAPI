using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class SequenceJobs : OrderingJobs
    {
        private List<Job> _jobs;
        private readonly bool _hasJobs;
        private string _orderedJobs = "";

        public SequenceJobs(string jobs)
        {
            _hasJobs = jobs != "";
            _jobs = CreateJobList(jobs.Split('|'));
        }

        public string GetJobSequence()
        {
            //move try here
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
                    throw new SelfReferenceException();
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
