using Domain.Abstract;
using System;
using System.IO;
using System.Xml;

namespace BusinessLayer.Implementation.ValidationStrategies
{
    /// <summary>
    /// Provides basic strategy for indicating if XML Document can be processed by the validator
    /// </summary>
    public abstract class XmlDocumentValidationStrategy : DocumentValidationStrategy<XmlReaderSettings>, IXmlDocumentValidationStrategy
    {
        /// <summary>
        /// Initializes the class using IXmlDocumentValidator
        /// </summary>
        /// <param name="documentValidator">The validator is used to process the document against current validation strategy</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if the validator is not provided</exception>
        public XmlDocumentValidationStrategy(IXmlDocumentValidator documentValidator) : base(documentValidator) { }

        /// <summary>
        /// Creates XmlDocument for preparation stage 
        /// </summary>
        /// <param name="stream">The stream represents the XML document for validation process</param>
        /// <returns>Created XML Document using the stream</returns>
        protected virtual XmlDocument CreateDocument(Stream stream)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);
            stream.Seek(0, SeekOrigin.Begin);

            return xmlDocument;
        }
    }
}