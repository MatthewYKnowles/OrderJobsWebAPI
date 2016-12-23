using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class JobPermutations
    {
        public List<string> GetPermutatio(string jobList)
        {
            List<string> permutations = new List<string> {jobList};
            if (jobList.Length > 2)
            {
                string[] jobs = jobList.Split('|');
                Swap(jobs, 0, 1);
                string job2 = string.Join("|", jobs);
                permutations.Add(job2);
            }
            
            return permutations;
        }

        public void Swap(string[] list, int i, int j)
        {
            var t = list[i];
            list[i] = list[j];
            list[j] = t;
        }
    }
}
