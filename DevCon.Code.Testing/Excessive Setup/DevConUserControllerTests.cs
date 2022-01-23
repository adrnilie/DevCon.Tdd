using System.Threading.Tasks;
using DevCon.Code.Business.Repositories;
using DevCon.Code.Business.Services;
using DevCon.Code.Business.Storage.Entities;
using DevCon.Code.Common;
using DevCon.Code.Messaging.Handlers;
using DevCon.Code.Messaging.Repository;
using DevCon.Code.Models.Response;
using DevCon.Code.Testing.Common.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DevCon.Code.Testing.Excessive_Setup
{
    public class DevConUserControllerTests
    {
        private readonly Mock<IDevConUserRepository> _devConUserRepositoryMock;
        private readonly Mock<IMessagingRepository> _messagingRepositoryMock;
        public DevConUserControllerTests()
        {
            _messagingRepositoryMock = new Mock<IMessagingRepository>();
            _devConUserRepositoryMock = new Mock<IDevConUserRepository>();
        }

        [Fact]
        public async Task CreateDevConUser_WhenValidRequest_ShouldCreateNewUser()
        {
            // Arrange

            var user = DevConUserEntityBuilder.ValidEntity();

            _devConUserRepositoryMock
                .Setup(x => x.CreateUserAsync(It.IsAny<DevConUser>()))
                .ReturnsAsync(user);

            var securePasswordService = new SecurePasswordService();
            var tokenProviderService = new TokenProviderService();

            var commandHandler = new CommandsHandler(_messagingRepositoryMock.Object);
            var devConUserService = new DevConUserService(_devConUserRepositoryMock.Object, securePasswordService);

            var controller = new DevConUserController(devConUserService, tokenProviderService, commandHandler);

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