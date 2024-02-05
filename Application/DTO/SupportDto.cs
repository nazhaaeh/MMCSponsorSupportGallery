using Microsoft.AspNetCore.Http;
using Application.Validations; 
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTO
{
    public class SupportDto
    {
        public Guid Idsuppport { get; set; }
        public string Namesupport { get; set; }
        [JsonIgnore]
        public IFormFile File { get; set; }
        public Guid SessionId { get; set; }

    }

    public class SupportCreateDto
    {
        public string Namesupport { get; set; }

        [FileConditions(10 * 1024 * 1024, new[] { ".pdf", ".docx" }, ErrorMessage = "Le fichier dépasse la taille maximale autorisée ou le format n'est pas autorisé.")]
        [JsonIgnore]
        public IFormFile File { get; set; }
        public Guid SessionId { get; set; }

    }

    public class SupportUpdateDto
    {
        public string Namesupport { get; set; }
        [FileConditions(10 * 1024 * 1024, new[] { ".pdf", ".docx" }, ErrorMessage = "Le fichier dépasse la taille maximale autorisée ou le format n'est pas autorisé.")]
        [JsonIgnore]
        public IFormFile File { get; set; }
        public Guid SessionId { get; set; }

    }
}
