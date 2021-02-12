using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gifter.Common
{
    public static class Guard
    {

        /// <summary>
        /// Guard which check if provided <paramref name="parameter"/> is null, empty or whitespace. If so then throws ArgumentNullException.
        /// </summary>
        /// <param name="parameter">Parameter to guard</param>
        /// <param name="parameterName">Optional name of parameter</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null, empty, or whitespace. </exception>
        public static void IsNullEmptyOrWhiteSpace(string parameter, string parameterName="Parameter")
        {
            if (string.IsNullOrWhiteSpace(parameter)) throw new ArgumentNullException($"{parameterName} is null, empty or whitespace.");
        }

        public static void IsNull(object objectParam, string objectParamName  = "objectParam")
        {
            if (objectParam == null) throw new ArgumentNullException($"{objectParamName} is null.");
        }


        public static void IsValidPath(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException($"File for given path: \"{path}\" could not be found or path is not valid");
        }
    }
}
