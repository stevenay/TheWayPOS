using System;
using System.Collections.Generic;

namespace TheWayPOS.BL
{
    class BusinessLogicException : Exception
    {
        public List<BusinessLogicError> errors { get; set; }

        /// <summary>
        /// Business Logic Exception
        /// </summary>
        public BusinessLogicException() : base() { }

        /// <summary>
        /// Business Logic Exception
        /// </summary>
        /// <param name="message">Message</param>
        public BusinessLogicException(string message) : base(message) { }

        /// <summary>
        /// Business Logic Exception
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public BusinessLogicException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Business Logic Exception
        /// </summary>
        /// <param name="info">SerializationInfo info</param>
        /// <param name="context">Context</param>
        protected BusinessLogicException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
