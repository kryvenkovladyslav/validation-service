using System.IO;

namespace Domain.Models
{
    /// <summary>
    /// The model represent a request to File Storage
    /// </summary>
    public sealed class RequestModel
    {
        /// <summary>
        /// The stream representing a current document
        /// </summary>
        public Stream DocumentStream { get; init; }

        /// <summary>
        /// The full path to a required file
        /// </summary>
        public string RequiredFilePath { get; init; }

        /// <summary>
        /// The constructor for initialization the instance
        /// </summary>
        public RequestModel(string requiredFilePath, Stream documentStream) 
        {
            this.DocumentStream = documentStream;
            this.RequiredFilePath = requiredFilePath;
        }
    }
}