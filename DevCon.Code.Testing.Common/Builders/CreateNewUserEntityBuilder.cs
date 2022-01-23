using System;
using DevCon.Code.Business.Models;

namespace DevCon.Code.Testing.Common.Builders
{
    public static class CreateNewUserEntityBuilder
    {
        public static CreateUserModel ValidEntity(Action<CreateUserModel> modify = null)
        {
            var entity = new CreateUserModel
            {
                Name = "John Doe",
                EmailAddress = "john.doe@test.com",
                Password = Guid.NewGuid().ToString("N")
            };
            
            modify?.Invoke(entity);

            return entity;
        }
    }
}