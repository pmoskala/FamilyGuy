using FamilyGuy.Accounts.AccountQuery.Model;
using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Infrastructure;
using FamilyGuy.Processes.UserRegistration.Contract;
using FamilyGuy.UserApi.Model;
using FamilyGuy.UserApi.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FamilyGuy.UserApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQuery _query;
        private readonly IAuthService _authenticationService;

        public AccountsController(ICommandBus commandBus, IQuery query, IAuthService authenticationService)
        {
            _commandBus = commandBus;
            _query = query;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAccount([FromBody] PostAccountModel model)
        {
            Guid userId = model.Id;
            try
            {
                await _commandBus.Send(new RegisterUserCommand
                {
                    Id = userId,
                    BaseUrl = BaseUrl.Current,
                    LoginName = model.LoginName,
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Password = model.Password,
                    TelephoneNumber = model.TelephoneNumber
                });
            }
            catch (LoginNameAlreadyUsedException ex)
            {
                //ModelState.AddModelError(); // todo think about it 
                ModelStateDictionary mds = new ModelStateDictionary();
                mds.AddModelError(nameof(ex.LoginName), $"The provided username {ex.LoginName} already exists");
                return Conflict(mds);
            }

            return Created(new Uri($"{BaseUrl.Current}/api/v1.0/confirmations/{userId}"), userId);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> PostAuthenticate([FromBody] PostUserAuthenticationModel user)
        {
            AuthenticatedUserReadModel token = await _authenticationService.Authenticate(user.UserName, user.Password);
            return Content(JsonSerializer.Serialize(token));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountReadModel>> GetAccount([FromRoute]Guid id)
        {
            AccountReadModel account = await _query.Query<Task<AccountReadModel>, Guid>(id);
            return Content(JsonSerializer.Serialize(account));
        }
    }
}
