using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    /// <summary>
    /// The validator for validating documents
    /// </summary>
    public interface IDocumentValidator
    {
        /// <summary>
        /// Asynchronously validates a specified document
        /// </summary>
        /// <returns>The final validation result with all provided validation erros</returns>
        public Task<ValidationResult> ValidateDocumentAsync(RequestModel model);
    }
}