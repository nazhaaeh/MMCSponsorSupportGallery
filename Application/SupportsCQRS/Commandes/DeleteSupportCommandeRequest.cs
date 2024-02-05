using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportsCQRS.Commandes
{
    public class DeleteSupportCommandeRequest :IRequest
    {
        public Guid Id { get; set; }
    }
}
