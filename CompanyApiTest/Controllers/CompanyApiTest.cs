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
    public class CompanyApiTest : TestBase
    {
        private readonly string url = "companies";
        private string employeeUrl = "/companies/companyId/employees";

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
                company1, company2, company3, company4,
            };
            var httpClient = TestBase.GetHttpClient();
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
                company1, company2, company3, company4,
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
                company1, company2, company3, company4,
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

        [Fact]
        public async Task Should_update_company_name_when_put_url()
        {
            //given
            var company1 = new Company("id1", "name1");
            CompaniesController.Companies = new List<Company>()
            {
                company1,
            };
            var httpClient = GetHttpClient();
            var companyExpect = new Company("id1", "nameUpdated");
            var putContent = new StringContent(JsonConvert.SerializeObject(companyExpect), Encoding.UTF8,
                "application/json");
            //when
            var responseMessage = await httpClient.PutAsync(url + "/id1", putContent);
            //then
            responseMessage.EnsureSuccessStatusCode();
            var responseJson = await responseMessage.Content.ReadAsStringAsync();
            var updatedCompany = JsonConvert.DeserializeObject<Company>(responseJson);
            Assert.Equal(companyExpect, updatedCompany);
            Assert.Equal(companyExpect, CompaniesController.Companies[0]);
        }

        [Fact]
        public async Task Should_del_company_with_employees_when_del_company()
        {
            //given
            var company1 = new Company("id1", "name1");
            var company2 = new Company("id2", "name2");
            var company3 = new Company("id3", "name3");
            var company4 = new Company("companyId", "name4");
            CompaniesController.Companies = new List<Company>()
            {
                company1, company2, company3, company4,
            };
            //given
            var employee1 = new Employee("id1", "companyId", "employee1", 2000);
            var employee2 = new Employee("id2", "companyId", "employee2", 2000);
            var employee3 = new Employee("id3", "companyId", "employee3", 2000);
            var employee4 = new Employee("id4", "otherCompanyId", "employee4", 2000);
            EmployeesController.Employees = new List<Employee>()
            {
                employee1, employee2, employee3, employee4,
            };
            var httpClient = TestBase.GetHttpClient();
            //when
            var responseMessage = await httpClient.DeleteAsync(url + "/companyId");
            //then
            responseMessage.EnsureSuccessStatusCode();
            Assert.Equal(3, CompaniesController.Companies.Count);
            Assert.Single(EmployeesController.Employees);
        }

        [Fact]
        public async Task Should_can_add_employee_when_call_post_api_given_company_not_add()
        {
            //given
            CompaniesController.Companies = new List<Company>()
            {
                new Company("companyId", "name2"),
            };
            var httpClient = GetHttpClient();
            var employee = new Employee("小甲", 2000.3);
            var requestContent = CovertEmployeesToContent(employee);
            //when
            var httpResponseMessage = await httpClient.PostAsync(employeeUrl, requestContent);
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
            var responseMessage = await httpClient.GetAsync(employeeUrl);
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
            var responseMessage =
                await httpClient.PutAsync(employeeUrl + "/id1", CovertEmployeesToContent(employeeExpect));
            //then
            responseMessage.EnsureSuccessStatusCode();
            var employJson = await responseMessage.Content.ReadAsStringAsync();
            var employeeUpdated = JsonConvert.DeserializeObject<Employee>(employJson);
            Assert.Equal(employeeExpect, employeeUpdated);
            Assert.Equal(employeeExpect, EmployeesController.Employees[0]);
        }

        [Fact]
        public async Task Should_del_employee_when_del_given_company_id_and_id()
        {
            //given
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
            var responseMessage = await httpClient.DeleteAsync(employeeUrl + "/id1");
            //then
            responseMessage.EnsureSuccessStatusCode();
            Assert.Equal(3, EmployeesController.Employees.Count);
        }

        private static StringContent CovertEmployeesToContent(Employee employee)
        {
            var employeeJson = JsonConvert.SerializeObject(employee);
            var requestContent = new StringContent(employeeJson, Encoding.UTF8, "application/json");
            return requestContent;
        }

        private static StringContent SerializeCompanyToJson(Company company)
        {
            var serializeObject = JsonConvert.SerializeObject(company);
            var content = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            return content;
        }
    }
}