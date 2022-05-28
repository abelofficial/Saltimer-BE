using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Saltimer.Api.Models;

namespace Saltimer.Api.Dto
{
    public class UpdateUserCommand : IRequest
    {
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [EmailAddressAttribute]
        public string EmailAddress { get; set; }

        [Required]
        [UrlAttribute]
        public string ProfileImage { get; set; }
    }

    public class UserToUpdateUserCommandProfile : Profile
    {
        public UserToUpdateUserCommandProfile()
        {
            CreateMap<UpdateUserCommand, User>();
        }
    }
}
