using System.Collections.Generic;
using System.Linq;

namespace OrderJobs.Algorithm
{
    public abstract class OrderingJobs
    {
        protected List<Job> _jobs;

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
    }
}