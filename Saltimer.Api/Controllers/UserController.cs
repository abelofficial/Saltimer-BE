#nullable disable
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saltimer.Api.Dto;
using Saltimer.Api.Queries;

namespace Saltimer.Api.Controllers
{
    public class UserController : BaseController
    {
        private IMediator _mediator;
        public UserController(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
             : base(mapper, authService, context)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUser(string? filterTerm)
        {
            //return await _context.User.ToListAsync();
            return Ok(await _mediator.Send(new GetAllUsersQuery() { Filter = filterTerm }));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            return Ok(await _mediator.Send(new GetUserByIdQuery() { Id = id }));

        }


    }
}
