using System.Threading.Tasks;
using DevCon.Code.Common;
using DevCon.Code.Messaging;
using DevCon.Code.Models.Response;
using DevCon.Code.Testing.Common;
using DevCon.Code.Testing.Common.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DevCon.Code.Testing.Multiple_Assertions
{
    public class DevConUserControllerTests : TestableSystem
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CreateDevConUser_WhenEmailNullOrEmpty_ShouldReturnBadRequestResult(string emailAddress)
        {
            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity(x =>
            {
                x.EmailAddress = emailAddress;
            });

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<BadRequestObjectResult>(actionResult.Result);

            // Assert
            EnsureBadRequestStatusCode(response);
            response.ErrorMessage.Should().Be(ErrorMessages.Validation.EmailAddressRequired);
        }

        [Theory]
        [InlineData("test.mail")]
        [InlineData("@mail.com")]
        public async Task CreateDevConUser_WhenInvalidEmailFormat_ShouldReturnBadRequestResult(string emailAddress)
        {
            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity(x =>
            {
                x.EmailAddress = emailAddress;
            });

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<BadRequestObjectResult>(actionResult.Result);

            // Assert
            EnsureBadRequestStatusCode(response);
            response.ErrorMessage.Should().Be(ErrorMessages.Validation.InvalidEmailAddress);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CreateDevConUser_WhenPasswordNullOrEmpty_ShouldReturnBadRequestResult(string password)
        {
            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity(x =>
            {
                x.Password = password;
            });

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<BadRequestObjectResult>(actionResult.Result);

            // Assert
            EnsureBadRequestStatusCode(response);
            response.ErrorMessage.Should().Be(ErrorMessages.Validation.PasswordRequired);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CreateDevConUser_WhenConfirmPasswordNullOrEmpty_ShouldReturnBadRequestResult(string confirmPassword)
        {
            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity(x =>
            {
                x.ConfirmPassword = confirmPassword;
            });

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<BadRequestObjectResult>(actionResult.Result);

            // Assert
            EnsureBadRequestStatusCode(response);
            response.ErrorMessage.Should().Be(ErrorMessages.Validation.ConfirmPasswordRequired);
        }

        [Fact]
        public async Task CreateDevConUser_WhenPasswordsDoNotMatch_ShouldReturnBadRequestResult()
        {
            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity(x =>
            {
                x.Password = "password1";
                x.ConfirmPassword = "password2";
            });

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<BadRequestObjectResult>(actionResult.Result);

            // Assert
            EnsureBadRequestStatusCode(response);
            response.ErrorMessage.Should().Be(ErrorMessages.Validation.PasswordDontMatch);
        }

        [Fact]
        public async Task CreateDevConUser_WhenValidRequest_ShouldCreateNewUser()
        {
            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity();

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<OkObjectResult, DevConUserResponse>(actionResult.Result);

            // Assert
            EnsureSuccessStatusCode(response);
            response.Result.EmailAddress.Should().Be(request.EmailAddress);
            response.Result.Name.Should().Be(request.Name);
            response.Result.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateDevConUser_WhenUserCreated_ShouldNotHaveEmailConfirmed()
        {
            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity();

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<OkObjectResult, DevConUserResponse>(actionResult.Result);

            // Assert
            var dbUser = await FakeDevConUserRepository.FindUserAsync(response.Result.Id);
            dbUser.EmailConfirmed.Should().BeFalse();
        }

        [Fact]
        public async Task CreateDevConUser_WhenUserCreated_ShouldBeActive()
        {
            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity();

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<OkObjectResult, DevConUserResponse>(actionResult.Result);

            // Assert
            var dbUser = await FakeDevConUserRepository.FindUserAsync(response.Result.Id);
            dbUser.Active.Should().BeTrue();
        }

        [Fact]
        public async Task CreateDevConUser_WhenUserCreated_ShouldSendConfirmEmailCommand()
        {
            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity();

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<OkObjectResult, DevConUserResponse>(actionResult.Result);

            // Assert
            EnsureSuccessStatusCode(response);

            var message = GetSentMessages<SendUserConfirmEmailCommand>().Should().ContainSingle().Subject;
            message.CallbackUrl.Should().Contain(response.Result.Id.ToString("D"));
        }

        [Fact]
        public async Task CreateDevConUser_WhenUserCreated_ShouldSendUserCreatedToDataWarehouseCommand()
        {
            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity();

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<OkObjectResult, DevConUserResponse>(actionResult.Result);

            // Assert
            EnsureSuccessStatusCode(response);

            var message = GetSentMessages<SendUserCreatedToDataWarehouseCommand>().Should().ContainSingle().Subject;
            message.EmailAddress.Should().Be(response.Result.EmailAddress);
            message.Name.Should().Be(response.Result.Name);
        }

        [Fact]
        public async Task CreateDevConUser_WhenUserExists_ShouldReturnConflictObjectResult()
        {
            var existingUser = DevConUserEntityBuilder.ValidEntity();
            FakeDevConUserRepository.UpsertUsers(existingUser);

            // Arrange
            var request = DevConNewUserRequestBuilder.ValidEntity(x =>
            {
                x.EmailAddress = existingUser.EmailAddress;
            });

            // Act
            var actionResult = await DevConUserController.CreateDevConUser(request);
            var response = GetObjectResult<ConflictObjectResult>(actionResult.Result);

            // Assert
            EnsureConflictStatusCode(response);
            response.ErrorMessage.Should().Be(ErrorMessages.UserAlreadyExists);
        }
    }
}
