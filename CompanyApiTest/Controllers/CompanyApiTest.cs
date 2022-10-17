using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using CompanyApi.Controllers;
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
            CompaniesController.Companies = new List<Company>();
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

        [Fact]
        public async Task Should_return_400_when_name_duplicated()
        {
            //given
            CompaniesController.Companies = new List<Company>();
            var httpClient = GetHttpClient();
            var company = new Company("testCompany");
            var content = SerializeCompanyToJson(company);
            await httpClient.PostAsync(url, content);
            //when
            var rspMessage = await httpClient.PostAsync(url, content);
            //then
            Assert.Equal(HttpStatusCode.BadRequest, rspMessage.StatusCode);
        }

        [Fact]
        public async Task Should_return_all_companies_when_get_url()
        {
            //given
            var company1 = new Company("id1", "name1");
            var company2 = new Company("id2", "name2");
            var company3 = new Company("id3", "name3");
            var company4 = new Company("id4", "name4");
            CompaniesController.Companies = new List<Company>()
            {
                company1, company2, company3, company4
            };
            var httpClient = GetHttpClient();
            //when
            var rspMessage = await httpClient.GetAsync(url);
            rspMessage.EnsureSuccessStatusCode();
            var rspJson = await rspMessage.Content.ReadAsStringAsync();
            var companies = JsonConvert.DeserializeObject<List<Company>>(rspJson);
            Assert.Equal(4, companies.Count);
            Assert.Equal(company1, companies[0]);
            Assert.Equal(company2, companies[1]);
            Assert.Equal(company3, companies[2]);
            Assert.Equal(company4, companies[3]);
        }

        [Fact]
        public async Task Should_return_company_by_id_when_call_api_with_id()
        {
            //given
            var company1 = new Company("id1", "name1");
            var company2 = new Company("id2", "name2");
            var company3 = new Company("id3", "name3");
            var company4 = new Company("id4", "name4");
            CompaniesController.Companies = new List<Company>()
            {
                company1, company2, company3, company4
            };
            var httpClient = GetHttpClient();
            //when
            var responseMessage = await httpClient.GetAsync(url + "/id2");
            //then
            responseMessage.EnsureSuccessStatusCode();
            var contentJson = await responseMessage.Content.ReadAsStringAsync();
            var companyFind = JsonConvert.DeserializeObject<Company>(contentJson);
            Assert.Equal(company2, companyFind);
        }

        [Fact]
        public async Task Should_return_than_page_company_when_search_by_page_given_page_size_2_index_2()
        {
           //given
            var company1 = new Company("id1", "name1");
            var company2 = new Company("id2", "name2");
            var company3 = new Company("id3", "name3");
            var company4 = new Company("id4", "name4");
            CompaniesController.Companies = new List<Company>()
            {
                company1, company2, company3, company4
            };
            var httpClient = GetHttpClient();
            //when
            var responseMessage = await httpClient.GetAsync(url + "/pageSize/2/pages/2");
            //then
            responseMessage.EnsureSuccessStatusCode();
            var contentJson = await responseMessage.Content.ReadAsStringAsync();
            var companies = JsonConvert.DeserializeObject<List<Company>>(contentJson);
            Assert.Equal(2, companies.Count);
            Assert.Equal(company3, companies[0]);
            Assert.Equal(company4, companies[1]);
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