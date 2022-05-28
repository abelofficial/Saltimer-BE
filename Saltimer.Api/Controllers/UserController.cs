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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUser(string? filterTerm)
        {
            return Ok(await _mediator.Send(new GetAllUsersQuery() { Filter = filterTerm }));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            return Ok(await _mediator.Send(new GetUserByIdQuery() { Id = id }));

        }


    }
}
