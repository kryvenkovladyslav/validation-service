using System.IO;
using System.Xml;
using Domain.Abstract;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation
{
    /// <summary>
    /// Provides methods for validating XML Documents
    /// </summary>
    public class XmlDocumentValidator : IXmlDocumentValidator
    {
        /// <summary>
        /// Asynchronously validates XML Documents against predefined settings
        /// </summary>
        /// <param name="documentStream">The stream represents the XML document for validation process</param>
        /// <param name="settings">The settings are used by XML Reader while validating the document</param>
        public virtual async Task ValidateDocumentAsync(Stream documentStream, XmlReaderSettings settings)
        {
            using var reader = CreateValidationReader(documentStream, settings);

            while (await reader.ReadAsync()) { };
        }

        /// <summary>
        /// Creates a validation XML Reader for validation purposes
        /// </summary>
        /// <param name="documentStream">The stream represents the XML document for validation process</param>
        /// <param name="settings">The settings are used for creating XML Reader</param>
        /// <returns>Created XML Reader using the stream and provided settings</returns>
        protected virtual XmlReader CreateValidationReader(Stream documentStream, XmlReaderSettings settings)
        {
            return XmlReader.Create(documentStream, settings);
        }
    }
}