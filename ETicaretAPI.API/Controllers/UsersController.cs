using ETicaretAPI.Application.Features.Commands.AppUser.CeateUser;
using ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommanRequest createUserCommanRequest)
        {
            CreateUserCommandReponse response= await _mediator.Send(createUserCommanRequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommanRequest loginUserCommanRequest)
        {
            LoginUserCommanResponse response = await _mediator.Send(loginUserCommanRequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommanRequest)
        {
            GoogleLoginCommandResponse response = await _mediator.Send(googleLoginCommanRequest);
            return Ok(response);
        }
    }
}
