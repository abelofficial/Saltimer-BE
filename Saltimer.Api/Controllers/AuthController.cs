using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Saltimer.Api.Attributes;
using Saltimer.Api.Command;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Controllers
{
    [Route("api/[controller]"), Authorize]
    [Produces("application/json")]
    [ValidateTokenAttribute]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IMediator _mediator;
        public AuthController(IMediator mediator)

        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get currently logged in user.
        /// </summary>
        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
        public async Task<IActionResult> GetMe()
        {

            return Ok(await _mediator.Send(new GetCurrentUserQuery()));
        }

        /// <summary>
        /// Update current user info.
        /// </summary>
        [HttpPut("user")]
        public async Task<IActionResult> PutUser(UpdateUserCommand request)
        {
            await _mediator.Send(request);

            return NoContent();
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        [HttpPost("register"), AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserResponseDto), Description = "Newly created user data.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Register(RegisterUserCommand request)
        {

            return Ok(await _mediator.Send(request));
        }

        /// <summary>
        /// Authenticate  user.
        /// </summary>
        [HttpPost("login"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginUserCommand request)
        {

            return Ok(await _mediator.Send(request));
        }

    }
}