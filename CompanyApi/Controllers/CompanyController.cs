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
            if (!companies.Exists(company => company.Id == companyId))
            {
                return NotFound();
            }

            return new Employee(createRequest.Name, createRequest.Salary);
        }

        [HttpDelete]
        public void ClearAll()
        { 
            companies.Clear();
        }
    }
}