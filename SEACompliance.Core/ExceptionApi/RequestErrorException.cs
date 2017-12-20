using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SEACompliance.Core.ExceptionApi
{
    public class RequestErrorException:Exception
    {
        public RequestErrorException() : base() { }
        public RequestErrorException(string message) : base(message) { }
        public RequestErrorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public RequestErrorException(string message, System.Exception inner) : base(message, inner) { }
    }
}