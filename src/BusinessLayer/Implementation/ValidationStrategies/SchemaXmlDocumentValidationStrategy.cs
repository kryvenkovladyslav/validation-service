using System.IO;
using System.Xml;
using Domain.Abstract;
using BusinessLayer.Defaults;

namespace BusinessLayer.Implementation.ValidationStrategies
{
    /// <summary>
    /// Provides strategy for validating XML Document against XML Schema if the document can be processed by the validator
    /// </summary>
    public class SchemaXmlDocumentValidationStrategy : XmlDocumentValidationStrategy
    {
        /// <summary>
        /// Initializes the class using IXmlDocumentValidator
        /// </summary>
        /// <param name="documentValidator">The validator is used to process the document against current validation strategy</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if the validator is not provided</exception>
        public SchemaXmlDocumentValidationStrategy(IXmlDocumentValidator documentValidator) : base(documentValidator) { }

        /// <summary>
        /// Indicates if the required document can be processed against XML Schemas validation process
        /// </summary>
        /// <param name="documentStream">The stream represents the XML document for validation process</param>
        /// <returns>True if the document can be processed, otherwise False</returns>
        public override bool CanProcess(Stream documentStream)
        {
            var xmlDocument = CreateDocument(documentStream);
            using var schemas = GetDocumentSchemas(xmlDocument);

            return schemas.Count != 0;
        }

        /// <summary>
        /// Gets all XML Schemas mentioned in provided XML Document
        /// </summary>
        /// <param name="document">The document represents the XML document for validation process</param>
        /// <returns>Created a list of XML Schemas</returns>
        protected virtual XmlNodeList GetDocumentSchemas(XmlDocument document)
        {
            var root = document.DocumentElement;
            return root.SelectNodes(DefaultsEctd.SchemaXPath);
        }
    }
}