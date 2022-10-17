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
        private string url = "Company";

        [Fact]
        public async Task Should_can_add_company_when_call_post_api_given_company_not_add()
        {
            //given
            var testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var httpClient = testServer.CreateClient();
            var company = new Company("testCompany");
            var serializeObject = JsonConvert.SerializeObject(company);
            var content = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            //when
            var httpResponseMessage = await httpClient.PostAsync(url, content);
            //then
            httpResponseMessage.EnsureSuccessStatusCode();
            var rpsContentJson = await httpResponseMessage.Content.ReadAsStringAsync();
            var companyAdded = JsonConvert.DeserializeObject<Company>(rpsContentJson);
            Assert.NotNull(companyAdded.Id);
            Assert.Equal(company.Name, companyAdded.Name);
        }
    }
}