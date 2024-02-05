using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SupportsCQRS.Commandes
{
    public class CreateSupportCommandeHandler : IRequestHandler<CreateSupportCommandeRequest, SupportCreateDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment; 

        public CreateSupportCommandeHandler(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment; 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SupportCreateDto> Handle(CreateSupportCommandeRequest request, CancellationToken cancellationToken)
        {

            var support = _mapper.Map<Support>(request.SupportCreateRequest);

            support.SupportId = Guid.NewGuid();
            if (request.SupportCreateRequest.File != null && request.SupportCreateRequest.File.FileName != null)
            {
                string hosturl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}";
                var ImagName = $"Img_Support_{support.SupportId}";
                var ImagExten = Path.GetExtension(request.SupportCreateRequest.File.FileName);
                var localeFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot/uploads/", $"{ImagName}{ImagExten}");

                using (var fileStream = new FileStream(localeFilePath, FileMode.Create))
                {
                    await request.SupportCreateRequest.File.CopyToAsync(fileStream);
                }

                support.FilePath = $"{hosturl}/uploads/{ImagName}{ImagExten}";
            }

        _unitOfWork.Support.AddAsync(support);
            await _unitOfWork.SaveAsync();

            var supportDto = _mapper.Map<SupportCreateDto>(support);

            return supportDto;
        }
    }
}
