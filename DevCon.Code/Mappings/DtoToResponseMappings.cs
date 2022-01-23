using DevCon.Code.Business.Dto;
using DevCon.Code.Models.Response;

namespace DevCon.Code.Mappings
{
    public static class DtoToResponseMappings
    {
        public static DevConUserResponse AdaptToResponse(this DevConUserDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new DevConUserResponse
            {
                Id = dto.Id,
                EmailAddress = dto.EmailAddress,
                Name = dto.Name
            };
        }
    }
}