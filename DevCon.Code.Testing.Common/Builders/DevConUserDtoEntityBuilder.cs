using System;
using DevCon.Code.Business.Dto;

namespace DevCon.Code.Testing.Common.Builders
{
    public static class DevConUserDtoEntityBuilder
    {
        public static DevConUserDto ValidEntity(Action<DevConUserDto> modify = null)
        {
            var entity = new DevConUserDto
            {
                Id = Guid.NewGuid(),
                EmailAddress = "john.doe@test.com",
                Name = "John Doe",
                Active = true,
                EmailConfirmed = true,
                PasswordHash = Guid.NewGuid().ToString("N")
            };

            modify?.Invoke(entity);

            return entity;
        }
    }
}