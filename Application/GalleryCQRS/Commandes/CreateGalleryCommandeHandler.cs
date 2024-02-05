using Application.DTO;
using Application.Interfaces;
using Application.SupportsCQRS.Commandes;
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

namespace Application.GalleryCQRS.Commandes
{

    public class CreateGalleryCommandeHandler :IRequestHandler<CreateGalleryCommandeRequest, GalleryCreateDto>
    {
       
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateGalleryCommandeHandler(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
 
        }
            public async Task<GalleryCreateDto> Handle(CreateGalleryCommandeRequest request, CancellationToken cancellationToken)
            {
                var gallery = _mapper.Map<Gallery>(request.GalleryCreateRequest);

                gallery.GalleryId = Guid.NewGuid();
                if (request.GalleryCreateRequest.GalleryFile != null && request.GalleryCreateRequest.GalleryFile.FileName != null)
                {
                string hosturl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}";
                var ImagName = $"Img_Gallery_{gallery.GalleryId}";
                var ImagExten = Path.GetExtension(request.GalleryCreateRequest.GalleryFile.FileName);
            
                var localeFilePath = Path.Combine( _webHostEnvironment.ContentRootPath, "wwwroot/uploadsGallery/", $"{ImagName}{ImagExten}");

                    using (var fileStream = new FileStream(localeFilePath, FileMode.Create))
                    {
                        await request.GalleryCreateRequest.GalleryFile.CopyToAsync(fileStream);
                    }

                gallery.imageGalleryPath = $"{hosturl}/uploadsGallery/{ImagName}{ImagExten}";
                }

            _unitOfWork.Gallery.AddAsync(gallery);
                await _unitOfWork.SaveAsync();

                var gallerytDto = _mapper.Map<GalleryCreateDto>(gallery);

            return gallerytDto;

            }

        }
    }



