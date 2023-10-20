using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        [HttpGet("hello")]
        public string Get()
        {
            return "Hello World";
        }

        [HttpPost]
        public ActionResult<Company> CreateCompany(Company companyRequest)
        {
            Company companyCreated = new Company(companyRequest.Name);
            return companyCreated;
        }
    }
}
