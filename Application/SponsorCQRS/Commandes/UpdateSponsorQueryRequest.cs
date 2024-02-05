using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SponsorCQRS.Commandes
{
    public class UpdateSponsorQueryRequest:IRequest<SponsorUpdateDto>
    {
        public Guid Id { get; set; }
        public SponsorUpdateDto SponsorUpdateRequest { get; set; }

    }
}
