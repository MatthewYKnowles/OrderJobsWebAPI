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
        private readonly TestCaseDatabase _testCaseDatabase;
        public TestController()
        {
            _testCaseDatabase = new TestCaseDatabase();
        }
        [HttpGet]
        public async Task<string> Get([FromQuery]string url)
        {
            var client = new HttpClient();
            string jobs = "";
            IEnumerable<TestCases> testCaseList = _testCaseDatabase.GetTestCases();
                
            //magical string to call
            //http://localhost:55163/api/test?url=http://localhost:55163/api/values/
            foreach (var testCase in testCaseList)
            {
                HttpResponseMessage response = await client.GetAsync(url + testCase.TestCase);
                jobs += await response.Content.ReadAsStringAsync() + "\n";
            }
            return jobs;
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
