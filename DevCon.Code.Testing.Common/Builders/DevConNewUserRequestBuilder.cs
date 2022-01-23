using System;
using DevCon.Code.Models.Request;

namespace DevCon.Code.Testing.Common.Builders
{
    public static class DevConNewUserRequestBuilder
    {
        public static DevConNewUserRequest ValidEntity(Action<DevConNewUserRequest> modify = null)
        {
            var entity = new DevConNewUserRequest
            {
                EmailAddress = "john.doe@test.com",
                Name = "John Doe",
                Password = "TestPassword",
                ConfirmPassword = "TestPassword"
            };

            modify?.Invoke(entity);

            return entity;
        }
    }
}