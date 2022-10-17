using System.Collections.Generic;
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
        public List<Company> GetCompanies()
        {
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

        [HttpDelete]
        public void ClearAllCompanies()
        {
            companies.Clear();
        }
    }
}