using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Azure.Core;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SponsorCQRS.Commandes
{
    public class CreateSponsorQueryHandler : IRequestHandler<CreateSponsorQueryRequest, SponsorCreateDto>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CreateSponsorQueryHandler(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork,IMapper mapper,IWebHostEnvironment webHostEnvironment)
        {
            this.httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

    }
        public async Task<SponsorCreateDto> Handle(CreateSponsorQueryRequest request, CancellationToken cancellationToken)
        {
            var sponsor = _mapper.Map<Sponsor>(request.SponsorCreateRequest);

            sponsor.SponsorId = Guid.NewGuid();
            if (request.SponsorCreateRequest.SponsorImage != null && request.SponsorCreateRequest.SponsorImage.FileName != null)
            {
                string hosturl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}";
                var ImagName = $"Img_Sponsor_{sponsor.SponsorId}";
                var ImagExten = Path.GetExtension(request.SponsorCreateRequest.SponsorImage.FileName);
                var localeFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/UplaodSponsorImage/", $"{ImagName}{ImagExten}");

                using (var fileStream = new FileStream(localeFilePath, FileMode.Create))
                {
                    await request.SponsorCreateRequest.SponsorImage.CopyToAsync(fileStream);
                }

                sponsor.ImagesponsorPath = $"{hosturl}/UplaodSponsorImage/{ImagName}{ImagExten}";
            }

            _unitOfWork.Sponsor.AddAsync(sponsor);
            await _unitOfWork.SaveAsync();

            var sponsorDto = _mapper.Map<SponsorCreateDto>(sponsor);

            return sponsorDto;
        }
    }
}


