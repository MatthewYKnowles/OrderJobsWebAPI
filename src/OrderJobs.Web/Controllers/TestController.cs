using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            var orderJobsStorage = new OrderJobsStorage();
            orderJobsStorage.TestCases.InsertOneAsync("Test");
            //var blogContext = new BlogContext();
            //var post = new Post
            //await blogContext.Posts.InsertOneAsync(post);
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
