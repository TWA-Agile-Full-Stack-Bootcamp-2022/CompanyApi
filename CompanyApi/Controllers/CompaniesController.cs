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
    }
}