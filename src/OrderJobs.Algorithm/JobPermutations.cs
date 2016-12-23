using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class JobPermutations
    {
        public List<string> GetPermutations(string jobList)
        {
            List<string> permutations = new List<string> {jobList};
            if (jobList.Length > 2)
            {
                string[] jobs = jobList.Split('|');
                var temp = jobs[0];
                jobs[0] = jobs[1];
                jobs[1] = temp;
                string job2 = string.Join("|", jobs);
                permutations.Add(job2);
            }
            
            return permutations;
        }
    }
}
