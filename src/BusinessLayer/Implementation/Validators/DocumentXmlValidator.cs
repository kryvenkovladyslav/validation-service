using System;
using System.IO;
using System.Xml;
using Domain.Models;
using Domain.Abstract;
using System.Xml.Schema;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation.Validators
{
    /// <summary>
    /// The abstract XML document validator for validation XML documents
    /// </summary>
    public abstract class DocumentXmlValidator : IDocumentValidator
    {
        /// <summary>
        /// The final validation result
        /// </summary>
        private ValidationResult validationResult;

        /// <summary>
        /// The implementation of Validation Settings Provider
        /// </summary>
        private readonly IValidationXmlSettingProvider<DocumentXmlValidator> settingProvider;

        /// <summary>
        /// The constructor for initialization an instance
        /// </summary>
        /// <param name="settingProvider">The required implementation of Validation Settings Provider</param>
        public DocumentXmlValidator(IValidationXmlSettingProvider<DocumentXmlValidator> settingProvider)
        {
            this.validationResult = new ValidationResult();
            this.settingProvider = settingProvider ?? throw new ArgumentNullException(nameof(settingProvider));
        }

        /// <summary>
        /// Asynchronously validate the required XML document
        /// </summary>
        /// <returns>The final validation result</returns>
        public virtual async Task<ValidationResult> ValidateDocumentAsync(RequestModel model)
        {
            var validationSettings = await settingProvider.CreateXmlReaderSettingsAsync(model.RequiredFilePath, this.ValidationEventHandler);
            var reader = this.CreateValidationReader(model.DocumentStream, validationSettings);

            while (await reader.ReadAsync()) { };

            return this.validationResult;
        }

        /// <summary>
        /// Creates a XML document from a specified stream
        /// </summary>
        /// <param name="stream">A required document stream</param>
        /// <returns>Created XML document</returns>
        protected virtual XmlDocument CreateDocument(Stream stream)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);
            stream.Position = 0;

            return xmlDocument;
        }

        /// <summary>
        /// Handles all occurring validation errors
        /// </summary>
        protected virtual void ValidationEventHandler(object sender, ValidationEventArgs eventArgs)
        {
            this.validationResult.Errors.Add(new ValidationError
            {
                Description = eventArgs.Message,
                ErrorType = Enum.GetName(eventArgs.Severity)
            });
        }

        /// <summary>
        /// Creates XML validation reader
        /// </summary>
        /// <returns>XML validation reader</returns>
        protected virtual XmlReader CreateValidationReader(Stream documentStream, XmlReaderSettings settings)
        {
            return XmlReader.Create(documentStream, settings);
        }
    }
}