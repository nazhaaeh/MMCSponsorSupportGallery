using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SponsorCQRS.Commandes
{
   public class DeleteSponsorCommandeRequest : IRequest
    {
        public Guid Id { get; set; }
        
    }
}
