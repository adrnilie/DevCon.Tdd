using DevCon.Code.Business.Dto;
using DevCon.Code.Business.Models;

namespace DevCon.Code.Business.Mappings
{
    public static class ModelToDtoMappers
    {
        public static DevConUserDto AdaptToDto(this CreateUserModel userModel, string passwordHash)
        {
            if (userModel == null)
            {
                return null;
            }

            return new DevConUserDto
            {
                EmailAddress = userModel.EmailAddress,
                Name = userModel.Name,
                PasswordHash = passwordHash
            };
        }
    }
}