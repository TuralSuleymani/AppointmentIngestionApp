using AppointmentIngestion.API.Controllers;
using AppointmentIngestion.Common;
using AppointmentIngestion.Services.Contracts;
using AppointmentIngestion.Services.DTOs;
using AppointmentIngestion.Services.Validation;
using AppointmentIngestion.Tests.Data;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AppointmentIngestion.Tests.Unit.ControllerTests
{
    public class AppointmentsControllerTests
    {
        private readonly IAppointmentIngestionService _service;
        private readonly ILogger<AppointmentsController> _logger;
        private readonly AppointmentsController _controller;
        private readonly int _badRequestStatusCode = 400;

        public AppointmentsControllerTests()
        {
            _service = Substitute.For<IAppointmentIngestionService>();
            _logger = Substitute.For<ILogger<AppointmentsController>>();

            _controller = new AppointmentsController(_service, _logger)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                },
                ProblemDetailsFactory = GetProblemDetailsFactory()
            };
        }

        private static ProblemDetailsFactory GetProblemDetailsFactory()
        {
            var problemDetailsFactory = Substitute.For<ProblemDetailsFactory>();
            problemDetailsFactory.CreateProblemDetails(
                Arg.Any<HttpContext>(),
                Arg.Any<int?>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>())
                .Returns(callInfo => new ProblemDetails
                {
                    Status = callInfo.ArgAt<int?>(1),
                    Detail = callInfo.ArgAt<string>(4)
                });
            return problemDetailsFactory;
        }

        [Fact]
        public async Task Ingest_WhenSuccess_ShouldReturnCreated()
        {
            // Arrange
            int id = AppointmentData.ValidAppointmentId;
            var request = new AppointmentRequestDto();
            _service.IngestAsync(request)
                    .Returns(Task.FromResult(Result.Success<int, IDomainError>(id)));

            // Act
            var response = await _controller.Ingest(request);

            // Assert
            response.Should().BeOfType<CreatedAtActionResult>();
            var created = response as CreatedAtActionResult;

            created!.ActionName.Should().Be(nameof(AppointmentsController.GetById));
            created.RouteValues![nameof(id)].Should().Be(id);
            created.Value.Should().BeEquivalentTo(new
            {
                id,
                message = "Appointment created successfully."
            });
        }

        [Fact]
        public async Task Ingest_WhenFailure_ShouldReturnProperErrorResponse()
        {
            // Arrange
            var request = new AppointmentRequestDto();

            var error = Substitute.For<IDomainError>();
            var validationError = ErrorType.Unexpected;

            error.ErrorType.Returns(validationError);
            error.ErrorMessage.Returns(validationError.Name);
            error.Errors.Returns([ValidationErrors.AppointmentTimeSlot]);

            _service.IngestAsync(request)
                        .Returns(Result.Failure<int, IDomainError>(error));

            // Act
            var response = await _controller.Ingest(request);

            // Assert
            response.Should().BeOfType<BadRequestObjectResult>();
            var obj = response as BadRequestObjectResult;

            obj!.StatusCode.Should().Be(_badRequestStatusCode);

            obj.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAll_WhenSuccess_ShouldReturnOk()
        {
            // Arrange
            var items = AppointmentResponseDtoData.ValidAppointments;

            _service.GetAllAsync()
                        .Returns(Task.FromResult(Result.Success<IReadOnlyList<AppointmentResponseDto>, IDomainError>(items)));

            // Act
            var response = await _controller.GetAll();

            // Assert
            response.Should().BeOfType<OkObjectResult>();
            var ok = response as OkObjectResult;

            ok!.Value.Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task GetAll_WhenFailure_ShouldReturnErrorResponse()
        {
            // Arrange
            var error = Substitute.For<IDomainError>();
            var unexpectedError = ErrorType.Unexpected;

            error.ErrorType.Returns(unexpectedError);
            error.ErrorMessage.Returns(unexpectedError.Name);

            _service.GetAllAsync()
                        .Returns(Result.Failure<IReadOnlyList<AppointmentResponseDto>, IDomainError>(error));

            // Act
            var response = await _controller.GetAll();

            // Assert
            response.Should().BeOfType<BadRequestObjectResult>();
            var obj = response as BadRequestObjectResult;

            obj!.StatusCode.Should().Be(_badRequestStatusCode);

            obj.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_WhenSuccess_ShouldReturnOk()
        {
            // Arrange
            var appointment = AppointmentResponseDtoData.ValidAppointments[0];

            _service.GetByIdAsync(appointment.Id)
                        .Returns(Result.Success<AppointmentResponseDto,IDomainError>(appointment));

            // Act
            var response = await _controller.GetById(appointment.Id);

            // Assert
            response.Should().BeOfType<OkObjectResult>();
            var ok = response as OkObjectResult;

            ok!.Value.Should().BeEquivalentTo(appointment);
        }

        [Fact]
        public async Task GetById_WhenNotFound_ShouldReturn404()
        {
            // Arrange
            var error = Substitute.For<IDomainError>();
            var notFoundError = ErrorType.NotFound;

            error.ErrorType.Returns(notFoundError);
            error.ErrorMessage.Returns(notFoundError.Name);
            int appointmentId = AppointmentResponseDtoData.AppointmentId;

            _service.GetByIdAsync(appointmentId)
                        .Returns(Result.Failure<AppointmentResponseDto,IDomainError>(error));

            // Act
            var response = await _controller.GetById(appointmentId);

            // Assert
            response.Should().BeOfType<NotFoundObjectResult>();
            var obj = response as NotFoundObjectResult;

            obj!.StatusCode.Should().Be(404);
        }

    }

}
