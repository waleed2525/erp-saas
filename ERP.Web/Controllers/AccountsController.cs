using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using ERP.Application.Features.Financial.Commands.CreateAccount;

namespace ERP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountCommand cmd)
            => Ok(await _mediator.Send(cmd));
    }
}
