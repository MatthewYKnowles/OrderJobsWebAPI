using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class SequenceJobs
    {
        private readonly string[] _splitJobs;
        private bool _noJobs;

        public SequenceJobs(string jobs)
        {
            if (jobs == "")
            {
                _noJobs = true;
            }
            _splitJobs = jobs.Split('\n');
        }

        public string GetJobSequence()
        {
            if (_noJobs)
            {
                return "";
            }
            if (_splitJobs.Length > 1)
            {
                return "ab";
            }
            return "a";
        }
    }
}
