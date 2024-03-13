using System;

namespace AzureBlobStorage.Exceptions
{
    /// <summary>
    /// The AzureBlobNotExistException is thrown when a required Blob does not exist
    /// </summary>
    [Serializable]
    public class AzureBlobNotExistException : Exception
    {
        /// <summary>
        /// The basic constructor for initializing the class
        /// </summary>
        public AzureBlobNotExistException() { }

        /// <summary>
        /// The constructor for initializing the class using a specified message
        /// </summary>
        /// <param name="message"></param>
        public AzureBlobNotExistException(string message) : base(message) { }

        /// <summary>
        /// The constructor for initializing the class using an inner exception
        /// </summary>
        /// <param name="inner">The inner exception</param>
        public AzureBlobNotExistException(Exception inner) : base(ExceptionMessages.BlobDoesNotExistMessage, inner) { }
    }
}