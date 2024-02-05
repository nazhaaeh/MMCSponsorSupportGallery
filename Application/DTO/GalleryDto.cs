using Application.Validations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class GalleryDto
    {
        public Guid GalleryId { get; set; }
        [JsonIgnore]
        public IFormFile GalleryFile { get; set; }
        public Guid SessionId { get; set; }
    }
    public class GalleryCreateDto
    {
        [FileConditions(10 * 1024 * 1024, new[] { ".jpg", ".png", ".jpeg" }, ErrorMessage = "Le fichier dépasse la taille maximale autorisée ou le format n'est pas autorisé.")]
        [JsonIgnore]
        public IFormFile GalleryFile { get; set; }
        public Guid SessionId { get; set; }
    }

    public class GalleryUpdateDto
    {
        [FileConditions(10 * 1024 * 1024, new[] { ".jpg", ".png", ".jpeg" }, ErrorMessage = "Le fichier dépasse la taille maximale autorisée ou le format n'est pas autorisé.")]
        [JsonIgnore]
        public IFormFile GalleryFile { get; set; }
        public Guid SessionId { get; set; }
    }
}
