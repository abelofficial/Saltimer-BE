using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Saltimer.Api.Dto
{
    public class SignupUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email {get; set;} = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
