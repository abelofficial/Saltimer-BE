using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Saltimer.Api.Dto
{
    public class LoginUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
