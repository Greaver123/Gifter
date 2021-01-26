using Gifter.Services.Constants;

namespace Gifter.Services.Common
{
    public class OperationResult<T>
    {
        public string Message { get; internal set; }
        public OperationStatus Status { get; internal set; }
        public T Payload { get; internal set; }
    }
}
