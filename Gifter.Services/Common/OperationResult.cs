using Gifter.Services.Constants;
using System.Collections.Generic;

namespace Gifter.Services.Common
{
    public class OperationResult<T>
    {
        public OperationResult()
        {
            Errors = new List<OperationError>();
        }
        public string Message { get; internal set; }
        public OperationStatus Status { get; internal set; }
        public T Payload { get; internal set; }
        public IList<OperationError> Errors { get; set; }
    }
}
