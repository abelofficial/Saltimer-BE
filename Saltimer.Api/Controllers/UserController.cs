#nullable disable
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saltimer.Api.Attributes;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Controllers
{
    [Route("api/[controller]"), Authorize]
    [ValidateTokenAttribute]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponseDto>))]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers(string? filterTerm)
        {
            return Ok(await _mediator.Send(new GetAllUsersQuery() { Filter = filterTerm }));

        }

        /// <summary>
        /// Get a single user.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            return Ok(await _mediator.Send(new GetUserByIdQuery() { Id = id }));

        }

    }
}
