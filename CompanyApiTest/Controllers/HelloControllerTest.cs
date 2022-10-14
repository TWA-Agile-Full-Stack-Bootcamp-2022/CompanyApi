using System;
using System.Net.Http;
using System.Threading.Tasks;
using CompanyApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace CompanyApiTest.Controllers
{
    public class HelloControllerTest
    {
        [Fact]
        public async Task Should_return_hello_world_with_default_request()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            // when
            var response = await client.GetAsync("/hello");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // then
            Assert.Equal("Hello World", responseString);
        }
    }
}
