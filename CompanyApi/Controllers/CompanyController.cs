using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("/api/companies")]
    public class CompanyController : Controller
    {
        [HttpPost]
        public Company AddCompany(Company company)
        {
            return company;
        }
    }
}