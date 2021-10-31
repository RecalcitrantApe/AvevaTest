using AutoMapper;
using EmployeeApi.Application.Common.Interfaces;
using EmployeeApi.Application.Mappings;
using EmployeeApi.Domain.Entities;

namespace EmployeeApi.Application.Employees.Queries
{
    public record ComputerDto : IMapFrom<Computer>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Computer, ComputerDto>();
        }
    }
}
