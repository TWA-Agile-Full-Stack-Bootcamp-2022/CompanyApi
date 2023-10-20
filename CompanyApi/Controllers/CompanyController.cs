using Microsoft.AspNetCore.Mvc;

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
    }
}
