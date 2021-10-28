using System;

namespace EmployeeApi.Domain.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string email)
            : base($"Email string \"{email}\" is not valid.")
        {
        }
    }
}
