using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using OrderJobs.Algorithm;

namespace OrderJobs.Web
{
    public class OrderJobsStorage
    {
        private static IMongoClient _client;
        private static IMongoDatabase _database;
        public string testCasesCollection = "testcases";
        static OrderJobsStorage()
        {
            var connectionString = "mongodb://localhost:27017";
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("orderedjobs");
        }

        public IMongoCollection<Job> TestCases
        {
            get { return _database.GetCollection<Job>(testCasesCollection); }
        }

    }
}
