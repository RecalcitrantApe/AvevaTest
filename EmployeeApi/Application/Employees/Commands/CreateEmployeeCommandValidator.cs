using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApi.Application.Employees.Commands
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.FirstName).Length(2, 100);
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.LastName).Length(2, 150);
            RuleFor(x => x.Email).Length(2, 500);
            RuleFor(x => x.Email).Length(2, 100);
            RuleFor(x => x.DateOfBirth).NotNull();
            RuleFor(x => x.DateOfBirth).Must(BeDateInPast);
            RuleFor(x => x.Email).Must(BeValidEmail);
        }

        private bool BeValidEmail(string val)
        {
            return Application.Common.RegexUtilities.IsValidEmail(val);
        }

        private bool BeDateInPast(DateTime date)
        {
            return DateTime.UtcNow > date;
        }
    }
}
