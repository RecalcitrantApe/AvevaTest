using AutoMapper;
using EmployeeApi.Application.Common.Interfaces;
using EmployeeApi.Application.Mappings;
using EmployeeApi.Domain.Entities;
using EmployeeApi.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeApi.Application.Employees.Queries
{
    public record EmployeeDto : IMapFrom<Employee>
    {
        public int Id { get;  set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool CurrentlyEmployed { get; set; }

        public int NumberOfComputers
        {
            get
            {
                return Computers == null ? 0 : Computers.Count();
                
            }
        }


        public IList<ComputerDto> Computers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Employee, EmployeeDto>()
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(s => DateOnly.ToDate(s.DateOfBirth)))
                .ForMember(x => x.NumberOfComputers, opt => opt.MapFrom(s => s.Computers == null ? 0 : s.Computers.Count));
        }
    }

}
