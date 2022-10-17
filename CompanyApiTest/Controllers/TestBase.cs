using System.Net.Http;
using CompanyApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace CompanyApiTest.Controllers
{
    public class TestBase
    {
        public static HttpClient GetHttpClient()
        {
            var testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var httpClient = testServer.CreateClient();
            return httpClient;
        }
    }
}