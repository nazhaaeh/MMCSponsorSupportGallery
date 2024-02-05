using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Application.Validations
{
    public class FileConditions : ValidationAttribute
    {
        private readonly int _maxSizeInBytes;
        private readonly string[] _acceptedExtensions;

        public FileConditions(int maxSizeInBytes, string[] acceptedExtensions)
        {
            _maxSizeInBytes = maxSizeInBytes;
            _acceptedExtensions = acceptedExtensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null)
            {
                // Si le fichier est null, la validation réussit car cela peut être facultatif
                return ValidationResult.Success;
            }

            // Vérification de la taille
            if (file.Length >= _maxSizeInBytes)
            {
                return new ValidationResult($"Le fichier ne doit pas dépasser {_maxSizeInBytes / (1024 * 1024)} Mo.");
            }

            // Vérification de l'extension
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!_acceptedExtensions.Contains(fileExtension))
            {
                return new ValidationResult($"Le format du fichier n'est pas autorisé. Veuillez choisir un fichier avec une des extensions suivantes: {string.Join(", ", _acceptedExtensions)}.");
            }

            return ValidationResult.Success;
        }
    }
}
