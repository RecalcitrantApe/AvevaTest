using AutoMapper;
using EmployeeApi.Application.Common.Interfaces;
using EmployeeApi.Application.Employees.Queries;
using EmployeeApi.Application.Mappings;
using EmployeeApi.Domain.Entities;
using EmployeeApi.Infrastructure.Persistence;
using EmployeeApi.Web.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;

namespace Tests.Integration_tests
{
    public class MediatrQueryDbTests
    {
        private readonly Mock<ILogger<GetEmployeeQuery>> logger;
        private readonly Mock<IMediator> mockMediator;
        private readonly IConfigurationProvider configuration;
        private readonly IMapper mapper;


        public MediatrQueryDbTests()
        {
            this.logger = new Mock<ILogger<GetEmployeeQuery>>();
            this.mockMediator = new Mock<IMediator>();

            this.configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            this.mapper = configuration.CreateMapper();
        }

        [Test]
        public async Task GetEmployeeShouldReturnEmployee()
        {

            //Arange
            ICurrentUserService currentUserService = new DummyCurrentUserService();

            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase")
               .Options;
            using (var context = new EmployeeDbContext(options, currentUserService))
            {
                context.Employees.Add(new Employee { FirstName = "Test", LastName = "Testessen", DateOfBirth = EmployeeApi.Domain.ValueObjects.DateOnly.From(DateTime.Now), Email = EmployeeApi.Domain.ValueObjects.Email.From("test@test.com"), });
                context.SaveChanges();

                GetEmployeeQuery command = new GetEmployeeQuery(1);
                GetEmployeeQueryHandler handler = new GetEmployeeQueryHandler(context, this.mapper);

                //Act through mediator
                EmployeeDto employee = await handler.Handle(command, new System.Threading.CancellationToken());

                employee.Should().NotBeNull();
                
            }
        }

    }
}
