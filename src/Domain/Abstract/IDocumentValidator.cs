using Domain.Models;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Domain.Abstract
{
    /// <summary>
    /// Provides methods for validating documents against specified settings
    /// </summary>
    /// <typeparam name="TSettings">The settings are used while validating the document if needed</typeparam>
    public interface IDocumentValidator<TSettings> where TSettings : class
    {
        /// <summary>
        /// Asynchronously validates documents
        /// </summary>
        /// <param name="documentStream">The stream represents the document for validation process</param>
        /// <param name="settings">The settings are used while validating the document if needed</param>
        public Task ValidateDocumentAsync(Stream documentStream, TSettings settings = default);
    }

    /// <summary>
    /// Provides methods for validating XML Documents against specified XML Reader Settings
    /// </summary>
    public interface IXmlDocumentValidator : IDocumentValidator<XmlReaderSettings> { }
}