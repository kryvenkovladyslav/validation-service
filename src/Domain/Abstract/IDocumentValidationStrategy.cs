using System.IO;
using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IDocumentValidationStrategy
    {
        /// <summary>
        /// Provides a boolean result if the file can be processed 
        /// </summary>
        /// <param name="documentStream">A required document to be processed</param>
        /// <returns>True if a file can be processed</returns>
        public bool CanProcess(Stream documentStream);

        /// <summary>
        /// Asynchronously processes the file
        /// </summary>
        /// <returns>Provided validation result</returns>
        public Task<ValidationResult> ProcessAsync(RequestModel requestModel);
    }
}