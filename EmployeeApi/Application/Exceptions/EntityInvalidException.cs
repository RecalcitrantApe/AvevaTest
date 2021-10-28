using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Exceptions
{
    public class EntityInvalidException : Exception
    {

        public EntityInvalidException(string message)
               : base(message)
        {
        }

        public EntityInvalidException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public EntityInvalidException(string name, object key)
            : base($"Entity is in an invalid state: \"{name}\" ({key}).")
        {
        }
    }
}
