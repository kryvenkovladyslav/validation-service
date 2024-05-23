using System.IO;
using Domain.Abstract;

namespace BusinessLayer.Implementation.ValidationStrategies
{
    /// <summary>
    /// Provides strategy for validating XML Document against DTD if the document can be processed by the validator
    /// </summary>
    public class DtdXmlDocumentValidationStrategy : XmlDocumentValidationStrategy
    {
        /// <summary>
        /// Initializes the class using IXmlDocumentValidator
        /// </summary>
        /// <param name="documentValidator">The validator is used to process the document against current validation strategy</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if the validator is not provided</exception>
        public DtdXmlDocumentValidationStrategy(IXmlDocumentValidator documentValidator) : base(documentValidator) { }

        /// <summary>
        /// Indicates if the required document can be processed against DTD validation process
        /// </summary>
        /// <param name="documentStream">The stream represents the XML document for validation process</param>
        /// <returns>True if the document can be processed, otherwise False</returns>
        public override bool CanProcess(Stream documentStream)
        {
            var xmlDocument = CreateDocument(documentStream);
            var documentType = xmlDocument.DocumentType;

            return documentType?.PublicId != null || documentType?.SystemId != null;
        }
    }
}