using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gifter.Common.Exceptions
{
    /// <summary>
    /// Represents error that occured in FileService durning excution.
    /// </summary>
    public class FileServiceException: Exception
    {
        public FileServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
