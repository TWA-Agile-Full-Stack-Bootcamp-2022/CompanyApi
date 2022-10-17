using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("/api/companies")]
    public class CompanyController : Controller
    {
        private static List<Company> companies = new List<Company>();

        [HttpPost]
        public ActionResult<Company> AddCompany(Company newCompany)
        {
            var company = companies.Find(company => company.Name.Equals(newCompany.Name));
            if (company != null)
            {
                return BadRequest();
            }

            companies.Add(newCompany);
            return Ok(newCompany);
        }

        [HttpGet]
        public List<Company> GetCompanies([FromQuery] int? pageSize, [FromQuery] int? pageIndex)
        {
            if (pageSize != null && pageIndex != null)
            {
                return companies.Skip(pageSize.Value * (pageIndex.Value - 1)).Take(pageSize.Value).ToList();
            }

            return companies;
        }

        [HttpGet("{companyID}")]
        public ActionResult<Company> GetCompany(string companyID)
        {
            var company = GetCompanyByID(companyID);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpPut("{companyID}")]
        public ActionResult<Company> UpdateCompany(string companyID, Company updatedCompany)
        {
            var company = GetCompanyByID(companyID);
            if (company == null)
            {
                return NotFound();
            }

            company.UpdateBy(updatedCompany);
            return Ok(company);
        }

        [HttpPost("{companyID}/employees")]
        public ActionResult<Employee> AddNewEmployee(string companyID, Employee employee)
        {
            var company = GetCompanyByID(companyID);
            if (company == null)
            {
                return NotFound();
            }

            var addEmployee = company.AddEmployee(employee);
            return Ok(addEmployee);
        }

        [HttpGet("{companyID}/employees")]
        public ActionResult GetEmployees(string companyID)
        {
            var company = GetCompanyByID(companyID);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company.Employees);
        }

        [HttpGet("{companyID}/employees/{employeeID}")]
        public ActionResult<Employee> GetEmployee(string companyID, string employeeID)
        {
            var employee = GetEmployeeFromCompany(companyID, employeeID);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPut("{companyID}/employees/{employeeID}")]
        public ActionResult UpdateEmployee(string companyID, string employeeID, Employee modifiedEmployee)
        {
            var employee = GetEmployeeFromCompany(companyID, employeeID);
            if (employee == null)
            {
                return NotFound();
            }

            employee.UpdateBy(modifiedEmployee);
            return Ok(employee);
        }

        [HttpDelete("{companyID}/employees/{employeeID}")]
        public ActionResult DeleteEmployee(string companyID, string employeeID)
        {
            var company = companies.Find(company => company.CompanyID.Equals(companyID));
            if (company == null)
            {
                return NotFound();
            }

            if (company.FindEmployee(employeeID) == null)
            {
                return NotFound();
            }

            company.DeleteEmployee(employeeID);
            return Ok();
        }

        [HttpDelete]
        public void ClearAllCompanies()
        {
            companies.Clear();
        }

        [HttpDelete("{companyID}")]
        public void DeleteCompany(string companyID)
        {
            companies.RemoveAll(company => company.CompanyID.Equals(companyID));
        }

        private Company GetCompanyByID(string companyID)
        {
            return companies.Find(company1 => company1.CompanyID.Equals(companyID));
        }

        private Employee GetEmployeeFromCompany(string companyID, string employeeID)
        {
            var company = companies.Find(company => company.CompanyID.Equals(companyID));
            return company?.FindEmployee(employeeID);
        }
    }
}