namespace OrderJobs.Algorithm
{
    public class Job
    {
        public string Name { get; set; }
        public string Dependency { get; }

        public Job(string name, string dependency)
        {
            Name = name;
            Dependency = dependency;
        }
    }
}