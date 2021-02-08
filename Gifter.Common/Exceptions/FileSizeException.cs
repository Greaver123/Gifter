using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gifter.Common.Exceptions
{
    public class FileSizeException : Exception
    {
        public int MaxSize { get; private set; }

        public FileSizeException(string message, int maxSize) : base(message)
        {
            MaxSize = maxSize;
        }
    }
}
