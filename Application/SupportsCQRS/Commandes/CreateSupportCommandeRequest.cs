using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportsCQRS.Commandes
{
    public class CreateSupportCommandeRequest :IRequest<SupportCreateDto>
    {
        public SupportCreateDto SupportCreateRequest { get;}
        public CreateSupportCommandeRequest(SupportCreateDto supportCreateRequest)
        {
            SupportCreateRequest = supportCreateRequest;
        }
    }
}
