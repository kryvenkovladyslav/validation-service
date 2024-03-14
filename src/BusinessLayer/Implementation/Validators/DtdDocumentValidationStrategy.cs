using System.IO;
using Domain.Models;
using Domain.Abstract;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation.Validators
{
    /// <summary>
    /// The XML document validator for validation XML documents against DTD
    /// </summary>
    public class DtdDocumentValidationStrategy : DocumentXmlValidator, IDocumentValidationStrategy
    {
        /// <summary>
        /// The constructor for initialization an instance
        /// </summary>
        /// <param name="settingProvider">The required implementation of Validation Settings Provider</param>
        public DtdDocumentValidationStrategy(IValidationXmlSettingProvider<DtdDocumentValidationStrategy> settingProvider) : base(settingProvider) { }

        /// <summary>
        /// Provides a boolean result if the file can be processed 
        /// </summary>
        /// <param name="documentStream">A required document to be processed</param>
        /// <returns>True if a file can be processed</returns>
        public bool CanProcess(Stream documentStream)
        {
            var xmlDocument = this.CreateDocument(documentStream);
            var documentType = xmlDocument.DocumentType;

            return documentType?.PublicId != null || documentType?.SystemId != null;
        }

        /// <summary>
        /// Asynchronously processes the file
        /// </summary>
        /// <returns>Provided validation result</returns>
        public async Task<ValidationResult> ProcessAsync(RequestModel model)
        {
            var result = await this.ValidateDocumentAsync(model);
            return result;
        }
    }
}