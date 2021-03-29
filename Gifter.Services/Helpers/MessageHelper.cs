using Gifter.Services.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gifter.Services.Helpers
{
    public static class MessageHelper
    {

        /// <summary>
        /// Message entity not found for given id and entity type.
        /// </summary>
        /// <param name="entityName">Name of entity</param>
        /// <param name="id">Id of entity</param>
        /// <returns>Message: "<paramref name="entityName"/> with id = <paramref name="id"/> not found."</returns>
        public static string CreateEntityNotFoundMessage(string entityName, int id)
        {
            return $"{entityName} with id = {id} not found.";
        }

        /// <summary>
        /// Create operation "Error" message for given entity and operation type.
        /// </summary>
        /// <param name="entityName">Name of entity.</param>
        /// <returns>Message: Internal error. Could not <paramref name="operationType"/> "<paramref name="entityName"/>".</returns>
        public static string CreateOperationErrorMessage(string entityName, OperationType operationType)
        {
            return $"Internal error. Could not {operationType} \"{entityName}\".";
        }

        /// <summary>
        /// Create operation "success" message for given entity and operation type.
        /// </summary>
        /// <param name="entityName">Name of entity.</param>
        /// <returns>Message: "<paramref name="entityName"/>" updated successful.</returns>
        public static string CreateOperationSuccessMessage(string entityName, OperationType operationType)
        {
            return $"\"{entityName}\" {operationType} successful.";
        }
    }
}
