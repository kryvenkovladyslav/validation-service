using System.IO;
using System.Xml;
using Domain.Models;
using Domain.Abstract;
using BusinessLayer.Defaults;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation.Validators
{
    /// <summary>
    /// The XML document validator for validation XML documents against XML Schema
    /// </summary>
    public class SchemaDocumentValidationStrategy : DocumentXmlValidator, IDocumentValidationStrategy
    {
        /// <summary>
        /// The constructor for initialization an instance
        /// </summary>
        /// <param name="settingProvider">The required implementation of Validation Settings Provider</param>
        public SchemaDocumentValidationStrategy(IValidationXmlSettingProvider<SchemaDocumentValidationStrategy> settingProvider) : base(settingProvider) { }

        /// <summary>
        /// Provides a boolean result if the file can be processed 
        /// </summary>
        /// <param name="documentStream">A required document to be processed</param>
        /// <returns>True if a file can be processed</returns>
        public bool CanProcess(Stream documentStream)
        {
            var xmlDocument = this.CreateDocument(documentStream);
            using var schemas = this.GetDocumentSchemas(xmlDocument);

            return schemas.Count != 0;
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

        /// <summary>
        /// Gets all schemas for a current document
        /// </summary>
        /// <param name="document">A required document</param>
        /// <returns>A list of XML nodes</returns>
        private XmlNodeList GetDocumentSchemas(XmlDocument document)
        {
            var root = document.DocumentElement;
            return root.SelectNodes(DefaultsEctd.SchemaXPath);
        }
    }
}