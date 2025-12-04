using AppointmentIngestion.Repositories.Contracts;
using AppointmentIngestion.Repositories.Models;
using AppointmentIngestion.Services.Contracts;
using AppointmentIngestion.Services.DTOs;
using AppointmentIngestion.Services.Implementations;
using AppointmentIngestion.Services.Validation;
using AppointmentIngestion.Tests.Data;
using AppointmentIngestion.Tests.Unit.Factories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;

namespace AppointmentIngestion.Tests.Unit.Services
{
    public class AppointmentIngestionServiceTests
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _datetimeProvider;
        private readonly AppointmentRequestValidator _validator;
        private readonly AppointmentIngestionService _service;

        public AppointmentIngestionServiceTests()
        {
            _repository = Substitute.For<IAppointmentRepository>();
            _mapper = Substitute.For<IMapper>();
            _datetimeProvider = GetDatetimeProvider();
            _validator = new AppointmentRequestValidator(_datetimeProvider);
            _service = new AppointmentIngestionService(_repository, _validator, _mapper);
        }

        private static IDateTimeProvider GetDatetimeProvider()
        {
            var datetimeProvider = Substitute.For<IDateTimeProvider>();
            datetimeProvider.UtcNow.Returns(new DateTime(2025, 1, 1, 1, 0, 0, DateTimeKind.Utc));
            return datetimeProvider;
        }

        [Fact]
        public async Task IngestAsync_WhenClientNameIsEmpty_ShouldFail()
        {
            //Arrange
            var request = AppointmentRequestDtoFactory.Create(clientName: "");

            //Act
            var result = await _service.IngestAsync(request);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Errors.Should().Contain("ClientName is required.");

            await _repository.DidNotReceive().AddAsync(Arg.Any<Appointment>());
        }

        [Theory]
        [ClassData(typeof(InvalidAppointmentTimesData))]
        public async Task IngestAsync_WhenAppointmentTimeIsNotValid_ShouldFail(DateTime appointment)
        {
            //Arrange
            var request = AppointmentRequestDtoFactory.Create(appointmentTime: appointment);

            //Act
            var result = await _service.IngestAsync(request);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Errors.Should().Contain("Appointment must start on the hour or half-hour.");

            await _repository.DidNotReceive().AddAsync(Arg.Any<Appointment>());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task IngestAsync_WhenAppointmentNot5MinutesInFuture_ShouldFail(int minute)
        {
            //Arrange
            var request = AppointmentRequestDtoFactory.Create(appointmentTime: GetDatetimeProvider().UtcNow.AddMinutes(minute));

            //Act
            var result = await _service.IngestAsync(request);

            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Errors.Should().Contain("Appointment time must be at least 5 minutes in the future.");

            await _repository.DidNotReceive().AddAsync(Arg.Any<Appointment>());
        }

        [Theory]
        [InlineData(-4)]
        [InlineData(-34)]
        [InlineData(-1456)]
        public async Task IngestAsync_WhenServiceDurationIsNegative_ShouldFail(int sdim)
        {
            //Arrange
            var request = AppointmentRequestDtoFactory.Create(serviceDurationMinutes: sdim);

            //Arrange
            var result = await _service.IngestAsync(request);

            //Arrange
            result.IsFailure.Should().BeTrue();
            result.Error.Errors.Should().Contain("ServiceDurationMinutes must be positive.");

            await _repository.DidNotReceive().AddAsync(Arg.Any<Appointment>());
        }

        [Fact]
        public async Task IngestAsync_WhenAllRulesAreValid_ShouldIngest()
        {
            //Arrange
            int id = AppointmentData.ValidAppointmentId;

            var validTime = DateTime.UtcNow.AddMinutes(10)
                .AddSeconds(-DateTime.UtcNow.AddMinutes(10).Second)
                .AddMinutes(-(DateTime.UtcNow.AddMinutes(10).Minute % 30)); // normalize to 0 or 30

            var request = AppointmentRequestDtoFactory.Create(appointmentTime: validTime);

            var entity = new Appointment { Id = id };

            _mapper.Map<Appointment>(request).Returns(entity);

            _repository.AddAsync(entity).Returns(entity);

            //Act
            var result = await _service.IngestAsync(request);

            //Assert
            result.IsSuccess.Should().BeTrue();
            id.Should().Be(result.Value);

            await _repository.Received(1).AddAsync(entity);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentNotFound_ShouldFail()
        {
            // Arrange
            int id = AppointmentData.ValidAppointmentId;

            _repository.GetByIdAsync(id).Returns((Appointment?)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.ErrorMessage.Should().Be($"Appointment with ID {id} was not found.");
        }

        [Fact]
        public async Task GetByIdAsync_WhenAppointmentExists_ShouldReturnDto()
        {
            // Arrange
            int id = AppointmentData.ValidAppointmentId;

            var entity = new Appointment { Id = id };
            var dto = new AppointmentResponseDto { Id = id };

            _repository.GetByIdAsync(id).Returns(entity);
            _mapper.Map<AppointmentResponseDto>(entity).Returns(dto);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetAllAsync_WhenAppointmentsExist_ShouldReturnDtos()
        {
            // Arrange
            var entities = AppointmentData.ValidAppointments;

            var dtos = AppointmentResponseDtoData.ValidAppointments;

            int appointmentCount = dtos.Count;
            _repository.GetAllAsync().Returns(entities);
            _mapper.Map<IReadOnlyList<AppointmentResponseDto>>(entities).Returns(dtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Count.Should().Be(appointmentCount);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoAppointmentsExist_ShouldReturnEmptyList()
        {
            // Arrange
            var entities = new List<Appointment>();
            var dtos = new List<AppointmentResponseDto>();

            _repository.GetAllAsync().Returns(entities);
            _mapper.Map<IReadOnlyList<AppointmentResponseDto>>(entities).Returns(dtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEmpty();
        }


    }
}
