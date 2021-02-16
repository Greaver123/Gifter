using Gifter.Services.Constants;
using System;
using System.Collections.Generic;

namespace Gifter.Services.Common
{
    public class OperationResult<T>
    {
        public OperationResult()
        {
            Errors = new List<OperationError>();
        }

        public string Message { get; set; }
        public string Status { get; set; }
        public T Data { get; set; }
        public IList<OperationError> Errors { get; set; }
    }
}
