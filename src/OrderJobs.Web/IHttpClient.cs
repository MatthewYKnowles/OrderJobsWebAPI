using System.Net.Http;
using System.Threading.Tasks;

namespace OrderJobs.Web
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }

    class TestCaseHttpClient : IHttpClient
    {
        public Task<HttpResponseMessage> GetAsync(string url)
        {
            var httpClient = new HttpClient();
            return httpClient.GetAsync(url);
        }
    }
}