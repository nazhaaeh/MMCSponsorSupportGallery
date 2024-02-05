using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportsCQRS.Queries
{
   public class GetByIdSupportQueryHandler : IRequestHandler<GetBydSponsorQueryRequest,SupportDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetByIdSupportQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<SupportDto> Handle(GetBydSponsorQueryRequest request, CancellationToken cancellationToken)
        {
            var support = await _unitOfWork.Support.GetByIdAsync(request.id);

            if (support == null)
            {
                return null;
            }

            var supportDto = _mapper.Map<SupportDto>(support);
            return(supportDto);

        }
    }
}
