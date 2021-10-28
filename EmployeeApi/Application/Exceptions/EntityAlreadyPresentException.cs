using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Exceptions
{
    public class EntityAlreadyPresentException : Exception
    {
        public EntityAlreadyPresentException()
            : base()
        {
        }

        public EntityAlreadyPresentException(string message)
            : base(message)
        {
        }

        public EntityAlreadyPresentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public EntityAlreadyPresentException(string name, object key)
            : base($"Entity with values: \"{name}\" ({key}) already exist.")
        {
        }
    }
}
