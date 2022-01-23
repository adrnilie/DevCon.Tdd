using DevCon.Code.Business.Dto;
using DevCon.Code.Business.Storage.Entities;

namespace DevCon.Code.Business.Mappings
{
    public static class DtoToEntityMappers
    {
        public static DevConUser AdaptToEntity(this DevConUserDto userDto, string passwordSalt)
        {
            if (userDto == null)
            {
                return null;
            }

            return new DevConUser
            {
                Name = userDto.Name,
                EmailAddress = userDto.EmailAddress,
                PasswordHash = userDto.PasswordHash,
                PasswordSalt = passwordSalt
            };
        }
    }
}