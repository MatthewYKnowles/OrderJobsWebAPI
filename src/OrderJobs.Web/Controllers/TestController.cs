using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

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
        public Dictionary<int, TestCaseValidation> Get([FromQuery]string url)
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

        [HttpDelete("{command}")]
        public void Delete(string command)
        {
            if (command == "all")
            {
                _testCaseDatabase.Delete();
            }
        }
    }
}
