using Domain.Models;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public sealed class ValidationRequestModel
    {
        [NotNull]
        [Required]
        [FileExtensions(Extensions = nameof(FileExtensions.XML))]
        public string DocumentFullPath { get; init; }
    }
}