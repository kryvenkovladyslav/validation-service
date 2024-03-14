namespace Domain.Models
{
    /// <summary>
    /// The Validation Error represent a single error during XML validation
    /// </summary>
    public sealed class ValidationError
    {
        /// <summary>
        /// The type of the validation error
        /// </summary>
        public string ErrorType { get; init; }

        /// <summary>
        /// The main reason for providing a validation error
        /// </summary>
        public string Description { get; init; }
    }
}