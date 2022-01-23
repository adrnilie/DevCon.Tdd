using DevCon.Code.Business.Dto;
using DevCon.Code.Business.Storage.Entities;

namespace DevCon.Code.Business.Mappings
{
    internal static class EntityToDtoMappers
    {
        public static DevConUserDto AdaptToDto(this DevConUser user)
        {
            if (user == null)
            {
                return null;
            }

            return new DevConUserDto
            {
                Id = user.Id,
                EmailAddress = user.EmailAddress,
                PasswordHash = user.PasswordHash,
                Active = user.Active,
                EmailConfirmed = user.EmailConfirmed,
                Name = user.Name
            };
        }
    }
}