using Domain.Models;
using System.IO;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    /// <summary>
    /// Provides validation functionality for validation documents
    /// </summary>
    public interface IXmlValidationManager
    {
        /// <summary>
        /// Asynchronously validates the specified document
        /// </summary>
        /// <param name="documentFullPath">A path to a required document</param>
        /// <returns>The result representing the final validation status</returns>
        public Task<ValidationResult> ValidateDocumentAsync(string documentFullPath, Stream documentStream);
    }
}