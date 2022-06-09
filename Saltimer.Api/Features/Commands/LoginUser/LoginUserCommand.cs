using System.ComponentModel.DataAnnotations;
using MediatR;
using Saltimer.Api.Dto;

namespace Saltimer.Api.Command
{
    public class LoginUserCommand : IRequest<LoginResponse>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
