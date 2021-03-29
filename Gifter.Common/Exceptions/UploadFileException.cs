using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gifter.Common.Exceptions
{
    public class UploadFileException : Exception
    {
        public UploadFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
