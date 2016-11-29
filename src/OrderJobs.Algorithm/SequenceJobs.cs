using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class SequenceJobs
    {
        private readonly string[] _splitJobs;
        private bool _hasJobs;

        public SequenceJobs(string jobs)
        {
            _hasJobs = jobs != "";
            _splitJobs = jobs.Split('\n');
        }

        public string GetJobSequence()
        {
            return _hasJobs ? OrderJobs(): "";
        }

        private string OrderJobs()
        {
            string jobs = "";
            jobs += _splitJobs[0][0].ToString();

            if (_splitJobs.Length > 1)
            {
                jobs += _splitJobs[1][0].ToString();
            }
            return jobs;
        }
    }
}
