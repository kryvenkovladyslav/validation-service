using Domain.Abstract;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BusinessLayer.Implementation.ValidationStrategies
{
    public abstract class DocumentValidationStrategy<TSettings> : IDocumentValidationStrategy<TSettings> 
        where TSettings : class
    {
        /// <summary>
        /// The validator is used to process the document against current validation strategy
        /// </summary>
        private readonly IDocumentValidator<TSettings> documentValidator;

        /// <summary>
        /// Initializes the class using IDocumentValidator
        /// </summary>
        /// <param name="documentValidator">The validator is used to process the document against current validation strategy</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException is thrown if the validator is not provided</exception>
        public DocumentValidationStrategy(IDocumentValidator<TSettings> documentValidator)
        {
            this.documentValidator = documentValidator ?? throw new ArgumentNullException(nameof(documentValidator));
        }

        /// <summary>
        /// Asynchronously process the document if the document can be processed
        /// </summary>
        /// <param name="documentStream">The stream represents the document for validation process</param>
        /// <param name="settings">The settings are used by validator if needed while validating the document</param>
        public virtual async Task ProcessAsync(Stream documentStream, TSettings settings = default)
        {
            ArgumentNullException.ThrowIfNull(documentStream, nameof(documentStream));

            await this.documentValidator.ValidateDocumentAsync(documentStream, settings);
        }

        /// <summary>
        /// Indicates if the required document can be processed 
        /// </summary>
        /// <param name="documentStream">The stream represents the document for validation process</param>
        /// <returns>True if the document can be processed, otherwise False</returns>
        public abstract bool CanProcess(Stream documentStream);
    }
}