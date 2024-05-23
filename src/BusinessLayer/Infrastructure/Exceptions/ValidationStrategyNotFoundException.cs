using System;

namespace BusinessLayer.Infrastructure.Exceptions
{

    [Serializable]
    public sealed class ValidationStrategyNotFoundException : Exception
    {
        public ValidationStrategyNotFoundException() : this(ExceptionMessages.ValidatorNotFoundExceptionMessage) { }

        public ValidationStrategyNotFoundException(Exception inner) : base(ExceptionMessages.ValidatorNotFoundExceptionMessage, inner) { }

        public ValidationStrategyNotFoundException(string message) : base(message ?? ExceptionMessages.ValidatorNotFoundExceptionMessage) { }
    }
}