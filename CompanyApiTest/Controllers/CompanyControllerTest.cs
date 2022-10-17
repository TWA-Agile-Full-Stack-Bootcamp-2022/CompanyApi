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
        public async void Should_add_new_company_successfully()
        {
            var client = await ResetContextAndGetHttpClient();

            var company = new Company(name: "Benz");
            var stringContent = SerializeToJsonString(company);

            var response = await client.PostAsync("/api/companies", stringContent);

            response.EnsureSuccessStatusCode();
            var savedCompany = await DeserializeToType<Company>(response);
            Assert.Equal(company, savedCompany);
        }

        private static async Task<HttpClient> ResetContextAndGetHttpClient()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var client = server.CreateClient();
            return client;
        }

        private static async Task<T> DeserializeToType<T>(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        private static StringContent SerializeToJsonString<T>(T obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }
    }
}