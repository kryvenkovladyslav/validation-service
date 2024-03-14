using System;
using System.Collections.Generic;

namespace Domain.Models
{
    /// <summary>
    /// The final result of full XML validation process
    /// </summary>
    public sealed class ValidationResult
    {
        /// <summary>
        /// The list of all occurred validation errors
        /// </summary>
        public ICollection<ValidationError> Errors { get; init; }

        /// <summary>
        /// The status representing a result of validation process
        /// </summary>
        public string Status
        {
            get
            {
                return Enum.GetName(this.Errors.Count == 0 ? ValidationStatus.Passed : ValidationStatus.Failed);
            }
        }

        /// <summary>
        /// The default constructor for creating an instance
        /// </summary>
        public ValidationResult()
        {
            this.Errors = new List<ValidationError>();
        }
    }
}