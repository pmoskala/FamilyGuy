using Microsoft.AspNetCore.Mvc;

namespace FamilyGuy.UserApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountsController : Controller
    {
        public AccountsController()
        {

        }

        [HttpGet]
        public IActionResult GetAccount()
        {
            return Ok();
        }
    }
}
