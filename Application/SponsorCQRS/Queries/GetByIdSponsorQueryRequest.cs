using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SponsorCQRS.Queries
{
    public class GetByIdSponsorQueryRequest :IRequest<SponsorDto>
    {
        public Guid Id { get; set; }
    }
}
