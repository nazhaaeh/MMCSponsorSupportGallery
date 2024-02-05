using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SponsorCQRS.Queries
{
  public class GetSponsorQueryRequest :IRequest<List<SponsorDto>>
    {
    }
}
