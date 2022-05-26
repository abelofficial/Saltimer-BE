using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Saltimer.Api.Dto
{
    public class LoginUserCommand : IRequest<LoginResponse>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
