using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController : ControllerBase
    {
        private static List<Company> companies = new List<Company>();
        [HttpPost]
        public ActionResult<Company> Add(Company company)
        {
            company.Id = Guid.NewGuid().ToString();
            companies.Add(company);
            return new CreatedResult("/companies", company);
        }
    }
}