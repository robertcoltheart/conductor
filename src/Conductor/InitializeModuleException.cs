using System;
using System.Runtime.Serialization;

namespace Conductor
{
    /// <summary>
    /// The exception that is thrown when a module fails to initialize.
    /// </summary>
    [Serializable]
    public class InitializeModuleException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeModuleException"/> class.
        /// </summary>
        public InitializeModuleException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeModuleException"/> class with the specifed error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InitializeModuleException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeModuleException"/> class with the specifed error message
        /// and a reference to the inner exception that is the cause of the exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of the exception.</param>
        public InitializeModuleException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeModuleException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected InitializeModuleException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}