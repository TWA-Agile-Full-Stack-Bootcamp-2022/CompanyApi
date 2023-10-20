using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();

        [HttpGet("hello")]
        public string Get()
        {
            return "Hello World";
        }

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

        [HttpGet]
        public List<Company> GetAll()
        {
            return companies;
        }

        [HttpDelete]
        public void ClearAll()
        { 
            companies.Clear();
        }
    }
}