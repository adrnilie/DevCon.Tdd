using System;
using DevCon.Code.Business.Storage.Entities;

namespace DevCon.Code.Testing.Common.Builders
{
    public static class DevConUserEntityBuilder
    {
        public static DevConUser ValidEntity(Action<DevConUser> modify = null)
        {
            var entity = new DevConUser
            {
                Name = "John Doe",
                EmailAddress = "john.doe@test.com",
                Active = true,
                EmailConfirmed = true,
                PasswordHash = Guid.NewGuid().ToString("N"),
                PasswordSalt = Guid.NewGuid().ToString("N")
            };

            modify?.Invoke(entity);

            return entity;
        }
    }
}