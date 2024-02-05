using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Gallery
    {
        public Guid GalleryId { get; set; }

        public string imageGalleryPath { get; set; }
     
        public Guid SessionId { get; set; }
    }
}
