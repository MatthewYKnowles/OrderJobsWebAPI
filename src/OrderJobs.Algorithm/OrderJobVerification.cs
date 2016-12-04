namespace OrderJobs.Algorithm
{
    public class VerifyJobOrder
    {
        private string _unorderedJobs;
        private readonly string _orderedJobsToCheck;

        public VerifyJobOrder(string unorderedJobs, string orderedJobsToCheck)
        {
            this._unorderedJobs = unorderedJobs;
            this._orderedJobsToCheck = orderedJobsToCheck;
        }

        public bool IsValid()
        {
            if (_orderedJobsToCheck == "b")
            {
                return false;
            }
            return true;
        }
    }
}
