using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OrderJobs.Algorithm;
using MongoDB.Driver;


namespace OrderJobs.Web.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly ITestCaseDatabase _testCaseDatabase;
        private readonly OrderJobsPassFail _orderJobsPassFail;

        public TestController(ITestCaseDatabase testCaseDatabase, OrderJobsPassFail orderJobsPassFail)
        {
            _testCaseDatabase = testCaseDatabase;
            _orderJobsPassFail = orderJobsPassFail;
        }

        [HttpGet]
        public string Get([FromQuery]string url)
        {
            return _orderJobsPassFail.GetOrderedJobsPassFailResults(url).Result;
        }

        [HttpPost]
        public void Post([FromBody]TestCases testCase)
        {
            _testCaseDatabase.InsertTestCase(testCase);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
