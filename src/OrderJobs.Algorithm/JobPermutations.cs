using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class JobPermutations
    {
        private List<string> permutationsFromHeaps = new List<string>();
        public List<string> GetPermutatio(string jobList)
        {
            if (jobList.Length <= 2)
            {
                permutationsFromHeaps.Add(jobList);
            }
            else
            {
                string[] jobs = jobList.Split('|');
                Permute(jobs, jobs.Length);

            }
            return permutationsFromHeaps;
        }

        public void Swap(string[] list, int i, int j)
        {
            var t = list[i];
            list[i] = list[j];
            list[j] = t;
        }

        public void Permute(string[] list, int n)
        {
            if (n == 1)
            {
                permutationsFromHeaps.Add(string.Join("|", list));
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    Permute(list, n-1);
                    if (n % 2 == 1)
                    {
                        Swap(list, 0, n - 1);
                    }
                    else
                    {
                        Swap(list, i, n-1);
                    }
                }
            }
        }
    }
}
