using System;

namespace BusinessLayer.Infrastructure.Exceptions
{
    [Serializable]
    public sealed class ResourceNotFoundException : Exception
    {
        public string ResourcePath { get; init; }

        public ResourceNotFoundException(string resourcePath) : this()
        {
            this.ResourcePath = resourcePath;
        }

        public ResourceNotFoundException() : base(ExceptionMessages.ResourceNotFoundExceptionMessage) { }

        public ResourceNotFoundException(Exception inner) : base(ExceptionMessages.ResourceNotFoundExceptionMessage, inner) { }    

        public static void ThrowIf(bool condition, string resourcePath)
        {
            if (condition)
            {
                throw new ResourceNotFoundException(resourcePath);
            }      
        }
    }
}