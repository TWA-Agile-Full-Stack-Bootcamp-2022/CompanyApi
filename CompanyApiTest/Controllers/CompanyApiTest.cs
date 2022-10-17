using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Xunit;

namespace CompanyApiTest.Controllers
{
    public class CompanyApiTest
    {
        private string url = "companies";

        [Fact]
        public async Task Should_can_add_company_when_call_post_api_given_company_not_add()
        {
            //given
            var httpClient = GetHttpClient();
            var company = new Company("testCompany");
            var content = SerializeCompanyToJson(company);
            //when
            var httpResponseMessage = await httpClient.PostAsync(url, content);
            //then
            httpResponseMessage.EnsureSuccessStatusCode();
            var rpsContentJson = await httpResponseMessage.Content.ReadAsStringAsync();
            var companyAdded = JsonConvert.DeserializeObject<Company>(rpsContentJson);
            Assert.NotNull(companyAdded.Id);
            Assert.Equal(company.Name, companyAdded.Name);
            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);
        }

        private static StringContent SerializeCompanyToJson(Company company)
        {
            var serializeObject = JsonConvert.SerializeObject(company);
            var content = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            return content;
        }

        private static HttpClient GetHttpClient()
        {
            var testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var httpClient = testServer.CreateClient();
            return httpClient;
        }
    }
}