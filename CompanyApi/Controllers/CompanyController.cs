using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();

        [HttpPost]
        public ActionResult<Company> CreateCompany(Company companyRequest)
        {
            if (companies.Exists(company => company.Name.Equals(companyRequest.Name)))
            {
                return BadRequest();
            }

            Company companyCreated = new Company(companyRequest.Name);
            companies.Add(companyCreated);
            return companyCreated;
        }

        [HttpGet("{companyId}")]
        public ActionResult<Company> GetById(string companyId)
        {
            Company companyFound = companies.FirstOrDefault(company => company.Id == companyId);
            if (companyFound == null)
            { 
                return NotFound();
            }

            return companyFound;
        }

        [HttpGet]
        public List<Company> GetAll([FromQuery] int? pageSize, [FromQuery] int? pageIndex)
        {
            if (pageSize.HasValue && pageIndex.HasValue)
            {
                return companies
                    .Skip(pageSize.Value * (pageIndex.Value - 1))
                    .Take(pageSize.Value)
                    .ToList();
            }

            return companies;
        }

        [HttpPut("{companyId}")]
        public ActionResult PutCompany(string companyId, Company updateRequest)
        {
            Company companyToUpdate = companies.Find(company => company.Id.Equals(companyId));
            if (companyToUpdate == null)
            { 
                return NotFound();
            }

            companyToUpdate.Name = updateRequest.Name;
            return NoContent();
        }

        [HttpPost("{companyId}/employees")]
        public ActionResult<Employee> CreateEmployee(string companyId, Employee createRequest) 
        {
            Company company = companies.Find(company => company.Id == companyId);
            if (company == null)
            {
                return NotFound();
            }

            Employee employee = new Employee(createRequest.Name, createRequest.Salary);
            company.Employees.Add(employee);
            return employee;
        }

        [HttpGet("{companyId}/employees")]
        public List<Employee> GetAllEmployees(string companyId) 
        {
            Company company = companies.Find(company => company.Id.Equals(companyId));
            return company.Employees;
        }

        [HttpPut("{companyId}/employees/{employeeId}")]
        public ActionResult UpdateEmployee(string companyId, string employeeId, Employee updateRequest)
        {
            Company company = companies.Find(company => company.Id.Equals(companyId));
            if (company == null)
            {
                return NotFound();
            }

            Employee employee = company.Employees.Find(employee => employee.Id.Equals(employeeId));
            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = updateRequest.Name;
            employee.Salary = updateRequest.Salary;
            return NoContent();
        }

        [HttpDelete("{companyId}/employees/{employeeId}")]
        public ActionResult DeleteEmployee(string companyId, string employeeId)
        {
            Company company = companies.Find(company => company.Id.Equals(companyId));
            if (company == null)
            {
                return NotFound();
            }

            Employee employee = company.Employees.Find(employee => employee.Id.Equals(employeeId));
            if (employee == null)
            {
                return NotFound();
            }

            company.Employees.Remove(employee);
            return NoContent();
        }

        [HttpDelete]
        public void ClearAll()
        { 
            companies.Clear();
        }
    }
}