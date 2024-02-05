using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportsCQRS.Commandes
{
    public class UpdateSupportCommandeRequest : IRequest<SupportUpdateDto>
    {
        public Guid SupportId { get; set; } 
        public SupportUpdateDto SupportUpdateRequest { get; set; }
    }
}
