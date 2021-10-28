using System;

namespace EmployeeApi.Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
            : base()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public EntityNotFoundException(string name, object key)
            : base($"Could not find entity: \"{name}\" ({key}) was not found.")
        {
        }
    }
}
