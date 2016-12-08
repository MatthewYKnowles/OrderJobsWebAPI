﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OrderJobs.Algorithm;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderJobs.Web.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        // GET: api/test
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //look at all test cases in db
            //

            //var client = new HttpClient();
            //client.GetAsync(http:// + id + testcase (from db), )
            Console.WriteLine(id);
            return "testCases";
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody]TestCases testCases)
        {
            IMongoClient _client = new MongoClient();
            IMongoDatabase _database = _client.GetDatabase("orderedjobs");
            var collection = _database.GetCollection<TestCases>("testcases");
            await collection.InsertOneAsync(testCases);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
