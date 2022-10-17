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

        [Fact]
        public async void Should_return_BadRequest_when_add_new_company_with_the_same_name()
        {
            var client = await ResetContextAndGetHttpClient();

            var company = new Company(name: "Benz");
            var stringContent = SerializeToJsonString(company);

            await client.PostAsync("/api/companies", stringContent);
            var response = await client.PostAsync("/api/companies", stringContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void Should_return_all_companies_successfully()
        {
            var client = await ResetContextAndGetHttpClient();

            var companyBenz = new Company(name: "Benz");
            await client.PostAsync("/api/companies", SerializeToJsonString(companyBenz));
            var companyBMW = new Company(name: "BMW");
            await client.PostAsync("/api/companies", SerializeToJsonString(companyBMW));

            var response = await client.GetAsync("/api/companies");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var gotCompanies = await DeserializeToType<List<Company>>(response);
            Assert.Equal(new List<Company>() { companyBenz, companyBMW }, gotCompanies);
        }

        private static async Task<HttpClient> ResetContextAndGetHttpClient()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var client = server.CreateClient();
            await ClearAllCompanies(client);
            return client;
        }

        private static async Task ClearAllCompanies(HttpClient client)
        {
            await client.DeleteAsync("/api/companies");
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