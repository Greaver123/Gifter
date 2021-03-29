using Gifter.Services.Constants;
using System;
using System.Collections.Generic;

namespace Gifter.Services.Common
{
    public class OperationResult<T>
    {
        public OperationResult()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public string Message { get; set; }
        public string Status { get; set; }
        public T Data { get; set; }
        public IDictionary<string, List<string>> Errors { get; set; }
    }
}
