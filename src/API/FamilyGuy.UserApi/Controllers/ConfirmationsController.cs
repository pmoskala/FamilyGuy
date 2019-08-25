using FamilyGuy.Contracts.Communication.Interfaces;
using FamilyGuy.Processes.UserRegistration.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FamilyGuy.UserApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ConfirmationsController : Controller
    {
        private readonly ICommandBus _commandBus;

        public ConfirmationsController(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfirmation([FromRoute]Guid id, ConfirmationModel model)
        {
            try
            {
                await _commandBus.Send(new ConfirmUserCommand
                {
                    ConfirmationId = id,
                    Confirmed = model.Confirmed
                });
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            return Ok();
        }
    }

    public class ConfirmationModel
    {
        public bool Confirmed { get; set; }
    }
}
