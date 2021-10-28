using EmployeeApi.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeApi.Domain.Entities
{
    public class Employee : AbstractAuditable
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Email Email { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public bool CurrentlyEmployed { get; set; }


        public ICollection<Computer> Computers { get; set; } = new List<Computer>();

        public Employee()
        {
            
        }
        

    }
}
