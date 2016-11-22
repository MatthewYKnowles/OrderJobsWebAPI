using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderJobs.Algorithm
{
    public class OrderJobs
    {
        private readonly string _jobs;

        public OrderJobs(string jobs)
        {
            _jobs = jobs;
        }

        public string Order()
        {
            if (_jobs == "")
            {
                return "";
            }
            return _jobs[0].ToString();
        }
    }
}
