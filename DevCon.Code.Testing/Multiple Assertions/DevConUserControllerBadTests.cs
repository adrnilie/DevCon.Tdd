using System.Threading.Tasks;
using DevCon.Code.Messaging;
using DevCon.Code.Models.Response;
using DevCon.Code.Testing.Common;
using DevCon.Code.Testing.Common.Builders;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DevCon.Code.Testing.Multiple_Assertions
{
    public class DevConUserControllerBadTests : TestableSystem
    {
        [Fact]
        public async Task CreateDevConUser_WhenValidRequest_ShouldCreateUser()
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

            var dbUser = await FakeDevConUserRepository.FindUserAsync(response.Result.Id);
            dbUser.EmailConfirmed.Should().BeFalse();
            dbUser.Active.Should().BeTrue();

            var confirmEmailMessage = GetSentMessages<SendUserConfirmEmailCommand>().Should().ContainSingle().Subject;
            confirmEmailMessage.CallbackUrl.Should().Contain(response.Result.Id.ToString("D"));

            var dataWarehouseMessage = GetSentMessages<SendUserCreatedToDataWarehouseCommand>().Should().ContainSingle().Subject;
            dataWarehouseMessage.EmailAddress.Should().Be(response.Result.EmailAddress);
            dataWarehouseMessage.Name.Should().Be(response.Result.Name);
        }
    }
}