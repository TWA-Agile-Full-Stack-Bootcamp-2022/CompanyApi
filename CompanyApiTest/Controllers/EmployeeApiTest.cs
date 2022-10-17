using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using CompanyApi.Controllers;
using Newtonsoft.Json;
using Xunit;

namespace CompanyApiTest.Controllers
{
    public class EmployeeApiTest : TestBase
    {
        private string url = "/companies/companyId/employees";

        [Fact]
        public async Task Should_can_add_company_when_call_post_api_given_company_not_add()
        {
            //given
            CompaniesController.Companies = new List<Company>()
            {
                new Company("companyId", "name2"),
            };
            var httpClient = GetHttpClient();
            var employee = new Employee("小甲", 2000.3);
            var employeeJson = JsonConvert.SerializeObject(employee);
            var requestContent = new StringContent(employeeJson, Encoding.UTF8, "application/json");
            //when
            var httpResponseMessage = await httpClient.PostAsync(url, requestContent);
            //then
            httpResponseMessage.EnsureSuccessStatusCode();
            var rpsContentJson = await httpResponseMessage.Content.ReadAsStringAsync();
            var employeeAdded = JsonConvert.DeserializeObject<Employee>(rpsContentJson);
            Assert.NotNull(employeeAdded.Id);
            Assert.Equal(employee.Name, employeeAdded.Name);
            Assert.Equal(employee.Name, employeeAdded.Name);
            Assert.Equal(employee.Salary, employeeAdded.Salary);
            Assert.Equal("companyId", employeeAdded.CompanyId);
            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);
        }
    }
}