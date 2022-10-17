using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();

        public static List<Company> Companies
        {
            get => companies;
            set => companies = value;
        }

        [HttpPost]
        public ActionResult<Company> Add(Company company)
        {
            if (companies.Any(addedCompany => addedCompany.Name.Equals(company.Name)))
            {
                return BadRequest();
            }

            company.Id = Guid.NewGuid().ToString();
            companies.Add(company);
            return new CreatedResult("/companies", company);
        }

        [HttpGet]
        public List<Company> List()
        {
            return Companies;
        }

        [HttpGet("{id}")]
        public Company Get(string id)
        {
            return companies.First(company => id.Equals(company.Id));
        }

        [HttpGet("pageSize/{pageSize}/pages/{pageIndex}")]
        public List<Company> PagingGet(int pageSize, int pageIndex)
        {
            var startIndex = pageSize * (pageIndex - 1);
            return companies.GetRange(startIndex, pageSize);
        }

        [HttpPut("{id}")]
        public Company Update(string id, Company company)
        {
            var needUpdateCompany = companies.First(addedCompany => addedCompany.Id.Equals(id));
            needUpdateCompany.Name = company.Name;
            return needUpdateCompany;
        }

        [HttpDelete("{id}")]
        public void Del(string id)
        {
            var employees = EmployeesController.Employees;
            employees.Where(employee => id.Equals(employee.CompanyId)).ToList()
                .ForEach(employee => employees.Remove(employee));
            var needDelCompany = companies.First(company => id.Equals(company.Id));
            companies.Remove(needDelCompany);
        }
    }
}