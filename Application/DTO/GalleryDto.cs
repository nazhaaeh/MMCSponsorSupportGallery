using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class GalleryDto
    {
        public int GalleryId { get; set; }
        public IFormFile GalleryFile { get; set; }
    }
    public class GalleryCreateDto
    {
        public IFormFile GalleryFile { get; set; }
    }

    public class GalleryUpdateDto
    {

        public IFormFile GalleryFile { get; set; }
    }
}
