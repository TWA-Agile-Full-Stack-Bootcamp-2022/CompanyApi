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

        [Fact]
        public async Task Should_return_the_company_when_get_by_given_Id()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();
            var responseCreateCompany = await client.PostAsync("api/companies", SerializeToJsonString(new Company("Apple")));
            var companyGiven = await DeserializeTo<Company>(responseCreateCompany);

            // when
            var response = await client.GetAsync($"api/companies/{companyGiven.Id}");
            // then
            var companyGetById = await DeserializeTo<Company>(response);
            Assert.Equal(companyGiven.Id, companyGetById.Id);
            Assert.Equal(companyGiven.Name, companyGetById.Name);
        }

        [Fact]
        public async Task Should_return_NOT_Found_when_get_by_id_given_a_not_existed_company_id()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();
            // when
            var response = await client.GetAsync($"api/companies/{new Company("Some Company Not Existed").Id}");
            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_the_compaines_in_page_when_get_all_given_page_paramaters()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();

            Company companyApple = new Company("Apple");
            Company companyGoogle = new Company("Google");
            Company companyMicrosoft = new Company("Microsoft");
            await client.PostAsync("api/companies", SerializeToJsonString(companyApple));
            await client.PostAsync("api/companies", SerializeToJsonString(companyGoogle));
            await client.PostAsync("api/companies", SerializeToJsonString(companyMicrosoft));

            // when
            var response = await client.GetAsync("api/companies?pageSize=2&pageIndex=2");
            response.EnsureSuccessStatusCode();
            List<Company> companiesFetched = await DeserializeTo<List<Company>>(response);
            Assert.Single(companiesFetched);
            Assert.Equal(companyMicrosoft.Name, companiesFetched[0].Name);
        }

        [Fact]
        public async Task Should_can_uppdate_compainy_info_when_update_by_given_id_and_info()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();
            Company companyGiven = new Company("Apple");
            var responseCreateCompany = await client.PostAsync("api/companies", SerializeToJsonString(companyGiven));
            companyGiven = await DeserializeTo<Company>(responseCreateCompany);
            Company companyForUpdate = new Company("Pineapple");

            // when
            var response = await client.PutAsync($"api/companies/{companyGiven.Id}", SerializeToJsonString(companyForUpdate));

            // then
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            responseCreateCompany = await client.GetAsync($"api/companies/{companyGiven.Id}");
            var companyUpdated = await DeserializeTo<Company>(responseCreateCompany);
            Assert.Equal(companyGiven.Id, companyUpdated.Id);
            Assert.Equal(companyForUpdate.Name, companyUpdated.Name);
        }

        [Fact]
        public async Task Should_return_NOT_Found_when_update_given_a_Not_existed_company_id()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();
            // when
            Company companyNotExisted = new Company("Some Company Not Existed");
            var response = await client.PutAsync($"api/companies/{companyNotExisted.Id}", SerializeToJsonString(companyNotExisted));
            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_the_employee_when_create_employee_given_a_employee_under_company()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();
            var createdCompanyResponse = await client.PostAsync("/api/companies", SerializeToJsonString(new Company("Thoughtworks")));
            var companyGiven = await DeserializeTo<Company>(createdCompanyResponse);

            Employee employeeGiven = new Employee(name: "Alice", salary: 2000);
            // when
            var response = await client.PostAsync($"/api/companies/{companyGiven.Id}/employees", SerializeToJsonString(employeeGiven));
            // then
            response.EnsureSuccessStatusCode();
            Employee employeeCreated = await DeserializeTo<Employee>(response);
            Assert.Equal(employeeGiven.Name, employeeCreated.Name);
            Assert.Equal(employeeGiven.Salary, employeeCreated.Salary);
        }

        [Fact]
        public async Task Should_return_NOT_found_when_create_employee_given_a_not_existed_company()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();
            Company companyNotExisted = new Company("Not exist company");
            Employee employeeGiven = new Employee(name: "Alice", salary: 2000);
            // when
            var response = await client.PostAsync($"/api/companies/{companyNotExisted.Id}/employees", SerializeToJsonString(employeeGiven));
            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_the_employees_when_get_all_employee_by_given_companyId()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();
            var createdCompanyResponse = await client.PostAsync("/api/companies",
                SerializeToJsonString(new Company("Thoughtworks")));
            var companyGiven = await DeserializeTo<Company>(createdCompanyResponse);

            Employee alice = new Employee(name: "Alice", salary: 2000);
            Employee bob = new Employee(name: "Bob", salary: 2000);
            await client.PostAsync($"/api/companies/{companyGiven.Id}/employees", SerializeToJsonString(alice));
            await client.PostAsync($"/api/companies/{companyGiven.Id}/employees", SerializeToJsonString(bob));
            // when
            var response = await client.GetAsync($"/api/companies/{companyGiven.Id}/employees");
            // then
            response.EnsureSuccessStatusCode();
            List<Employee> employees = await DeserializeTo<List<Employee>>(response);
            Assert.Equal(2, employees.Count);
            Assert.Equal(alice, employees[0]);
            Assert.Equal(bob, employees[1]);
        }

        [Fact]
        public async Task Should_update_the_employee_when_update_employee_by_given_companyId_and_employeeId()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();
            var createdCompanyResponse = await client.PostAsync("/api/companies",
                SerializeToJsonString(new Company("Thoughtworks")));
            var companyTW = await DeserializeTo<Company>(createdCompanyResponse);
            var createdEmployeeResponse = await client.PostAsync($"/api/companies/{companyTW.Id}/employees",
                SerializeToJsonString(new Employee(name: "Alice", salary: 2000)));
            var employeeAlice = await DeserializeTo<Employee>(createdEmployeeResponse);
            // when
            var response = await client.PutAsync($"/api/companies/{companyTW.Id}/employees/{employeeAlice.Id}", 
                SerializeToJsonString(new Employee("Alice-2", 9999)));
            // then
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var responseGetEmployees = await client.GetAsync($"/api/companies/{companyTW.Id}/employees");
            List<Employee> employees = await DeserializeTo<List<Employee>>(responseGetEmployees);
            Assert.Single(employees);
            Assert.Equal("Alice-2", employees[0].Name);
            Assert.Equal(9999, employees[0].Salary);
        }

        [Fact]
        public async Task Should_return_NOT_Found_when_update_employee_given_a_not_existed_employeeId()
        {
            // given
            HttpClient client = await BuildContextAndGetHttpClientAsync();
            var createdCompanyResponse = await client.PostAsync("/api/companies",
                SerializeToJsonString(new Company("Thoughtworks")));
            var companyTW = await DeserializeTo<Company>(createdCompanyResponse);
            Employee employeeNotExisted = new Employee("Not existed Employee", 0);

            // when
            var response = await client.PutAsync($"/api/companies/{companyTW.Id}/employees/{employeeNotExisted.Id}", 
                SerializeToJsonString(employeeNotExisted));
            // then
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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

        private static StringContent SerializeToJsonString<T>(T objectGiven)
        {
            return new StringContent(JsonConvert.SerializeObject(objectGiven), Encoding.UTF8, "application/json");
        }
    }
}
