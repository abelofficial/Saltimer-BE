using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Saltimer.Api.Dto
{
    public class DeleteSessionCommand : IRequest
    {
        [Required]

        public int Id { get; set; }

    }

}
