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
        /// <param name="entityType">Type of entity.</param>
        /// <param name="id">Id of entity</param>
        /// <returns>Message: "<paramref name="entityType"/> with id = <paramref name="id"/> not found."</returns>
        public static string EntityNotFoundMessage(Type entityType, int id)
        {
            return $"{nameof(Type)} with id = {id} not found.";
        }
    }
}
