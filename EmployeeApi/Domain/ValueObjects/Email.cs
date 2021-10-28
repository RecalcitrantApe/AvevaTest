using EmployeeApi.Domain.Common;
using EmployeeApi.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApi.Domain.ValueObjects
{
    public class Email : ValueObject
    {

        public string EmailString { get; private set; }

        static Email()
        {
        }

        private Email()
        {
        }

        private Email(string email)
        {
            EmailString = email;
        }

        public static Email From(string emailAsString)
        {
            var isValid = Application.Common.RegexUtilities.IsValidEmail(emailAsString);
            if (!isValid)
                throw new InvalidEmailException(emailAsString);

            Email email = new(emailAsString);

            return email;

        }

        public override string ToString()
        {
            return EmailString;
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailString;
        }
    }
}
