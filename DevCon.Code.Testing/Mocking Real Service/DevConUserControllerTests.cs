using System;
using System.Threading.Tasks;
using DevCon.Code.Business.Dto;
using DevCon.Code.Business.Models;
using DevCon.Code.Business.Services;
using DevCon.Code.Common;
using DevCon.Code.Messaging;
using DevCon.Code.Messaging.Handlers;
using DevCon.Code.Models.Response;
using DevCon.Code.Testing.Common.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DevCon.Code.Testing.Mocking_Real_Service
{
    public class DevConUserControllerTests
    {
        private readonly Mock<IDevConUserService> _devConUserServiceMock;
        private readonly Mock<ITokenProviderService> _tokenProviderServiceMock;
        private readonly Mock<ICommandsHandler> _commandsHandlerMock;

        public DevConUserControllerTests()
        {
            _tokenProviderServiceMock = new Mock<ITokenProviderService>();
            _commandsHandlerMock = new Mock<ICommandsHandler>();
            _devConUserServiceMock = new Mock<IDevConUserService>();
        }

        [Fact]
        public async Task CreateDevConUser_WhenValidRequest_ShouldCreateNewUser()
        {
            // Arrange
            var user = DevConUserDtoEntityBuilder.ValidEntity();

            _devConUserServiceMock
                .Setup(x => x.CreateUserAsync(It.IsAny<CreateUserModel>()))
                .ReturnsAsync(ServiceResponse<DevConUserDto>.WithResult(user));

            _tokenProviderServiceMock
                .Setup(x => x.GenerateConfirmEmailTokenAsync())
                .ReturnsAsync(Guid.NewGuid().ToString("N"));

            _commandsHandlerMock
                .Setup(x => x.SendUserConfirmEmailCommandAsync(It.IsAny<SendUserConfirmEmailCommand>()))
                .Returns(Task.CompletedTask);

            _commandsHandlerMock
                .Setup(x => x.SendUserCreatedToDataWarehouseCommandAsync(It.IsAny<SendUserCreatedToDataWarehouseCommand>()))
                .Returns(Task.CompletedTask);

            var controller = new DevConUserController(_devConUserServiceMock.Object, _tokenProviderServiceMock.Object, _commandsHandlerMock.Object);

            var request = DevConNewUserRequestBuilder.ValidEntity(x =>
            {
                x.EmailAddress = user.EmailAddress;
            });

            // Act
            var actionResult = await controller.CreateDevConUser(request);
            var response = GetObjectResult<OkObjectResult, DevConUserResponse>(actionResult.Result);

            // Assert
            response.Success.Should().BeTrue();
            response.Result.Id.Should().Be(user.Id);
            response.Result.EmailAddress.Should().Be(user.EmailAddress);
            response.Result.Name.Should().Be(user.Name);
        }

        private Response<TValue> GetObjectResult<T, TValue>(ActionResult actionResult)
            where T : ObjectResult
            where TValue : class
        {
            var objectResult = actionResult as T;

            return objectResult?.Value as Response<TValue>;
        }
    }
}