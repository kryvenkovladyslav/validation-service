using BusinessLayer.Infrastructure.Exceptions;
using Domain.Abstract;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BusinessLayer.Implementation
{
    public class XmlValidationManager : IXmlValidationManager
    {
        private readonly ValidationResult validationResult;

        private readonly IEnumerable<IXmlDocumentValidationStrategy> strategies;

        private readonly IEnumerable<IValidationXmlSettingProvider<IXmlDocumentValidationStrategy>> settingsProviders;

        public XmlValidationManager(IEnumerable<IValidationXmlSettingProvider<IXmlDocumentValidationStrategy>> settingsProviders, 
            IEnumerable<IXmlDocumentValidationStrategy> strategies)
        {
            this.validationResult = new ValidationResult();

            this.strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
            this.settingsProviders = settingsProviders ?? throw new ArgumentNullException(nameof(settingsProviders));
        }

        public async Task<ValidationResult> ValidateDocumentAsync(string documentFullPath, Stream documentStream)
        {
            var requiredStrategy = this.strategies
                .Where(validator => validator.CanProcess(documentStream))
                .FirstOrDefault();

            if(requiredStrategy == null)
            {
                throw new ValidationStrategyNotFoundException();
            }

            var strategyType = requiredStrategy.GetType();

            var settingsProvider = this.settingsProviders
                .Where(provider => provider.GetType().GetInterfaces().Any(type => type == typeof(IValidationXmlSettingProvider<>).MakeGenericType(strategyType)))
                .FirstOrDefault();

            var requiredValidationSettings = await settingsProvider.CreateSettingsAsync(documentFullPath, this.ValidationEventHandler);
            await requiredStrategy.ProcessAsync(documentStream, requiredValidationSettings);

            return this.validationResult;
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs eventArgs)
        {
            this.validationResult.Errors.Add(new ValidationError 
            {
                Description = eventArgs.Message,
                ErrorType = Enum.GetName(eventArgs.Severity)
            });
        }
    }
}