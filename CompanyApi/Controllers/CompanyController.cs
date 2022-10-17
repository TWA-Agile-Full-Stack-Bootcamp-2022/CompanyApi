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
            var foundCompany = companies.Find(company => company.Name.Equals(newCompany.Name));
            if (foundCompany != null)
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
            var foundCompany = companies.Find(company => company.CompanyID.Equals(companyID));
            if (foundCompany == null)
            {
                return NotFound();
            }

            return Ok(foundCompany);
        }

        [HttpPut("{companyID}")]
        public ActionResult<Company> UpdateCompany(string companyID, Company updatedCompany)
        {
            var foundCompany = companies.Find(company => company.CompanyID.Equals(companyID));
            if (foundCompany == null)
            {
                return NotFound();
            }

            foundCompany.UpdateBy(updatedCompany);
            return Ok(foundCompany);
        }
        
        [HttpDelete]
        public void ClearAllCompanies()
        {
            companies.Clear();
        }
    }
}