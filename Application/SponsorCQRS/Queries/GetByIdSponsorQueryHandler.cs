using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SponsorCQRS.Queries
{
    public class GetByIdSponsorQueryHandler : IRequestHandler<GetByIdSponsorQueryRequest, SponsorDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetByIdSponsorQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<SponsorDto> Handle(GetByIdSponsorQueryRequest request, CancellationToken cancellationToken)
        {
            var Sponsor = await _unitOfWork.Sponsor.GetByIdAsync(request.Id);

            if (Sponsor == null)
            {
                return null;
            }

            var SponsorDto = _mapper.Map<SponsorDto>(Sponsor);
            return (SponsorDto);
        }
    }
}
