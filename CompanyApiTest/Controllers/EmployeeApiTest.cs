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
    [Collection("Sequential")]
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
            var requestContent = CovertEmpoyeesToContent(employee);
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

        [Fact]
        public async Task Should_can_find_company_employees()
        {
            var employee1 = new Employee("id1", "companyId", "employee1", 2000);
            var employee2 = new Employee("id2", "companyId", "employee2", 2000);
            var employee3 = new Employee("id3", "companyId", "employee3", 2000);
            var employee4 = new Employee("id4", "otherCompanyId", "employee4", 2000);
            EmployeesController.Employees = new List<Employee>()
            {
                employee1, employee2, employee3, employee4,
            };
            var httpClient = GetHttpClient();
            //when
            var responseMessage = await httpClient.GetAsync(url);
            //then
            responseMessage.EnsureSuccessStatusCode();
            var responseJson = await responseMessage.Content.ReadAsStringAsync();
            var employees = JsonConvert.DeserializeObject<List<Employee>>(responseJson);
            Assert.Equal(3, employees.Count);
            Assert.Equal(employee1, employees[0]);
            Assert.Equal(employee2, employees[1]);
            Assert.Equal(employee3, employees[2]);
        }

        [Fact]
        public async Task Should_update_employee_when_put_given_id_and_company_id()
        {
            //given
            var employee1 = new Employee("id1", "companyId", "employee1", 2000);
            EmployeesController.Employees = new List<Employee>()
            {
                employee1,
            };
            var httpClient = GetHttpClient();
            var employeeExpect = new Employee("id1", "companyId", "张三", 3000);
            //when
            var responseMessage = await httpClient.PutAsync(url + "/id1", CovertEmpoyeesToContent(employeeExpect));
            //then
            responseMessage.EnsureSuccessStatusCode();
            var employJson = await responseMessage.Content.ReadAsStringAsync();
            var employeeUpdated = JsonConvert.DeserializeObject<Employee>(employJson);
            Assert.Equal(employeeExpect, employeeUpdated);
            Assert.Equal(employeeExpect, EmployeesController.Employees[0]);
        }

        private static StringContent CovertEmpoyeesToContent(Employee employee)
        {
            var employeeJson = JsonConvert.SerializeObject(employee);
            var requestContent = new StringContent(employeeJson, Encoding.UTF8, "application/json");
            return requestContent;
        }
    }
}