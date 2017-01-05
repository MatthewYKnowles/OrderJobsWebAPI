using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class JobPermutations
    {
        private List<string> permutations = new List<string>();
        public List<string> GetPermutations(string jobList)
        {
            if (jobList.Length <= 2)
            {
                permutations.Add(jobList);
            }
            else
            {
                string[] jobs = jobList.Split('|');
                Permute(jobs, jobs.Length);

            }
            return permutations;
        }

        public void Permute(string[] list, int n)
        {
            if (n == 1)
            {
                AddPermutationToList(list);
            }
            else
            {
                ChangeOrder(list, n);
            }
        }

        private void AddPermutationToList(string[] list)
        {
            permutations.Add(string.Join("|", list));
        }

        private void ChangeOrder(string[] list, int n)
        {
            for (int i = 0; i < n; i++)
            {
                Permute(list, n - 1);
                var indexOfValueToSwap = n % 2 == 0 ? i : 0;
                Swap(list, indexOfValueToSwap, n - 1);
            }
        }


        public void Swap(string[] list, int i, int j)
        {
            var t = list[i];
            list[i] = list[j];
            list[j] = t;
        }
    }
}
