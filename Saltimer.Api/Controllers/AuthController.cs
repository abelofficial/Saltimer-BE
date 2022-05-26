﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saltimer.Api.Command;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Controllers
{
    public class AuthController : BaseController
    {
        private IMediator _mediator;
        public AuthController(IMediator mediator, IMapper mapper, IAuthService authService, SaltimerDBContext context)
            : base(mapper, authService, context)
        {
            _mediator = mediator;
        }

        [HttpGet("user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
        public ActionResult<UserResponseDto> GetMe()
        {
            var currentUser = _authService.GetCurrentUser();
            var response = _mapper.Map<UserResponseDto>(currentUser);
            return Ok(response);
        }

        [HttpPut("user")]
        public async Task<IActionResult> PutUser(UpdateUserCommand request)
        {
            await _mediator.Send(request);

            return NoContent();
        }

        [HttpPost("register"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> Register(RegisterUserCommand request)
        {

            return Ok(await _mediator.Send(request));
        }

        [HttpPost("login"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login(LoginUserCommand request)
        {

            return Ok(await _mediator.Send(request));
        }

    }
}