using System;
using System.Xml;
using System.Xml.Schema;

namespace Domain.Abstract
{
    public interface IXmlValidationSettingsCreator { }

    public interface IXmlSchemaValidationSettingsCreator : IXmlValidationSettingsCreator
    {
        public XmlReaderSettings CreateValidationSettings(XmlSchemaSet schemaSet, Action<object, ValidationEventArgs> eventHandler);
    }

    public interface IDtdValidationSettingsCreator : IXmlValidationSettingsCreator
    {
        public XmlReaderSettings CreateValidationSettings(XmlUrlResolver xmlUrlResolver, Action<object, ValidationEventArgs> eventHandler);
    }

    public class DtdValidationSettingsCreator : IDtdValidationSettingsCreator
    {
        public XmlReaderSettings CreateValidationSettings(XmlUrlResolver xmlUrlResolver, Action<object, ValidationEventArgs> eventHandler)
        {
            var settings = new XmlReaderSettings();

            settings.Async = true;
            settings.XmlResolver = xmlUrlResolver;
            settings.ValidationType = ValidationType.DTD;
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.ValidationEventHandler += new ValidationEventHandler(eventHandler);

            return settings;
        }
    }

    public class XmlSchemaValidationSettingsCreator : IXmlSchemaValidationSettingsCreator
    {
        public XmlReaderSettings CreateValidationSettings(XmlSchemaSet schemaSet, Action<object, ValidationEventArgs> eventHandler)
        {
            var settings = new XmlReaderSettings();
            settings.Async = true;
            settings.Schemas = schemaSet;
            settings.ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints | XmlSchemaValidationFlags.ReportValidationWarnings;

            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(eventHandler);

            return settings;
        }
    }
}