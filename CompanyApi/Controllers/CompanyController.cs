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

        [HttpDelete]
        public void ClearAll()
        { 
            companies.Clear();
        }
    }
}