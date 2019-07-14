using FamilyGuy.Accounts;
using FamilyGuy.Accounts.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FamilyGuy.UserApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountsController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            Guid newGuid = Guid.NewGuid();
            await _userRepository.Add(new User(newGuid, "ala", "kotowska", "alko", "a@a.pl", "passw0rd"));
            User user = await _userRepository.Get(newGuid);
            return Json(user);
        }
    }
}
