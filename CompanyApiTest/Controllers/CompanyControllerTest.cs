using System.Collections.Generic;
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
            HttpClient client = await BuildContextAndGetHttpClientAsync();

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
            HttpClient client = await BuildContextAndGetHttpClientAsync();

            Company companyGiven = new Company("Apple");
            var requestContent = SerializeToJsonString(companyGiven);
            // when
            var response = await client.PostAsync("api/companies", requestContent);
            // then
            response.EnsureSuccessStatusCode();
            Company companyCreated = await DeserializeTo<Company>(response);
            Assert.Equal(companyGiven.Name, companyCreated.Name);
        }

        [Fact]
        public async Task Should_return_badrequest_when_post_given_a_existed_company_name()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();

            Company companyGiven = new Company("Apple");
            await client.PostAsync("api/companies", SerializeToJsonString(companyGiven));
            // when
            var response = await client.PostAsync("api/companies", SerializeToJsonString(companyGiven));
            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_all_compaines_when_get_all()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();

            Company companyApple = new Company("Apple");
            Company companyGoogle = new Company("Google");
            await client.PostAsync("api/companies", SerializeToJsonString(companyApple));
            await client.PostAsync("api/companies", SerializeToJsonString(companyGoogle));

            // when
            var response = await client.GetAsync("api/companies");
            // then
            response.EnsureSuccessStatusCode();
            List<Company> companiesFetched = await DeserializeTo<List<Company>>(response);
            Assert.Equal(2, companiesFetched.Count);
            Assert.Equal(companyApple.Name, companiesFetched[0].Name);
            Assert.Equal(companyGoogle.Name, companiesFetched[1].Name);
        }

        private static async Task<T> DeserializeTo<T>(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        private static async Task<HttpClient> BuildContextAndGetHttpClientAsync()
        {
            TestServer server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            HttpClient client = server.CreateClient();
            await client.DeleteAsync("api/companies");
            return client;
        }

        private static StringContent SerializeToJsonString(Company companyGiven)
        {
            return new StringContent(JsonConvert.SerializeObject(companyGiven), Encoding.UTF8, "application/json");
        }
    }
}
