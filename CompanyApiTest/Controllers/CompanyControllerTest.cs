using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using CompanyApi.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CompanyApiTest.Controllers
{
    public class CompanyControllerTest
    {
        [Fact]
        public async Task Should_return_hello_world_with_default_request()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            // when
            var response = await client.GetAsync("api/companies/hello");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // then
            Assert.Equal("Hello World", responseString);
        }

        [Fact]
        public async Task Should_create_and_return_the_company_when_post_by_given_a_company_request()
        {
            // given
            TestServer server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            HttpClient client = server.CreateClient();

            Company companyGiven = new Company("Apple");
            var requestContent = SerializeToJsonString(companyGiven);
            // when
            var response = await client.PostAsync("api/companies", requestContent);
            // then
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            Company companyCreated = JsonConvert.DeserializeObject<Company>(responseBody);
            Assert.Equal(companyGiven.Name, companyCreated.Name);
        }

        private static StringContent SerializeToJsonString(Company companyGiven)
        {
            return new StringContent(JsonConvert.SerializeObject(companyGiven), Encoding.UTF8, "application/json");
        }
    }
}
