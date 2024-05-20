using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Domain.Abstract
{
    // <summary>
    /// Provides methods for indicating if the document can be processed by the validator
    /// </summary>
    public interface IDocumentValidationStrategy<TSettings>
        where TSettings : class
    {
        /// <summary>
        /// Indicates if the required document can be processed 
        /// </summary>
        /// <param name="documentStream">The stream represents the document for validation process</param>
        /// <returns>True if the document can be processed, otherwise False</returns>
        public bool CanProcess(Stream documentStream);

        /// <summary>
        /// Asynchronously process the document if the document can be processed
        /// </summary>
        /// <param name="documentStream">The stream represents the document for validation process</param>
        /// <param name="settings">The settings are used by Reader while validating the document if needed</param>
        public Task ProcessAsync(Stream documentStream, TSettings settings = default);
    }

    // <summary>
    /// Provides methods for indicating if the document can be processed by the validator
    /// </summary>
    public interface IXmlDocumentValidationStrategy : IDocumentValidationStrategy<XmlReaderSettings>
    { }
}