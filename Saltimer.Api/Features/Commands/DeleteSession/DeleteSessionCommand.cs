using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Saltimer.Api.Command
{
    public class DeleteSessionCommand : IRequest
    {
        [Required]

        public int Id { get; set; }

    }

}
