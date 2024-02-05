using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportsCQRS.Queries
{
    public class GetBydSponsorQueryRequest : IRequest<SupportDto>
    {
       public Guid id {  get; set; }
    }
}
