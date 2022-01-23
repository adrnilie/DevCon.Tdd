using System.Threading.Tasks;
using DevCon.Code.Business.Services;
using DevCon.Code.Testing.Common.Builders;
using DevCon.Code.Testing.Common.Fakes;
using FluentAssertions;
using Xunit;

namespace DevCon.Code.Testing.Pretending_To_Test
{
    public class DevConUserServiceTests
    {
        private readonly FakeDevConUserRepository _fakeDevConUserRepository;

        public DevConUserServiceTests()
        {
            _fakeDevConUserRepository = new FakeDevConUserRepository();
        }

        [Fact]
        public async Task CreateUserAsync_WhenValidRequest_ShouldCreateNewUser()
        {
            // Arrange
            var user = CreateNewUserEntityBuilder.ValidEntity();

            var securePasswordService = new SecurePasswordService();
            var sut = new DevConUserService(_fakeDevConUserRepository, securePasswordService);

            // Act
            var createdUser = await sut.CreateUserAsync(user);

            // Assert
            createdUser.Result.EmailAddress.Should().Be(user.EmailAddress);
            createdUser.Result.Name.Should().Be(user.Name);
        }
    }
}