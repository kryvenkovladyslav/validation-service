using System.Xml;
using System.Xml.Schema;

namespace Domain.Abstract
{
    /// <summary>
    /// The settings provider for XML validation
    /// </summary>
    /// <typeparam name="TValidator">A type of validator that accepts a provided settings</typeparam>
    public interface IValidationXmlSettingProvider<out TValidator> : IValidationSettingProvider<TValidator, XmlReaderSettings, ValidationEventArgs>
        where TValidator : IDocumentValidator
    { }
}