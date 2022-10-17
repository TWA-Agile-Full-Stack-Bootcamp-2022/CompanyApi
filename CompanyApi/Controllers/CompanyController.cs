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
        
        [HttpDelete]
        public void ClearAllCompanies()
        {
            companies.Clear();
        }
    }
}