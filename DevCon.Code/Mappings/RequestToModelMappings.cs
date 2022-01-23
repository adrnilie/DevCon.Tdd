using DevCon.Code.Business.Models;
using DevCon.Code.Models.Request;

namespace DevCon.Code.Mappings
{
    public static class RequestToModelMappings
    {
        public static CreateUserModel AdaptToModel(this DevConNewUserRequest request)
        {
            if (request == null)
            {
                return null;
            }

            return new CreateUserModel
            {
                Name = request.Name,
                EmailAddress = request.EmailAddress,
                Password = request.Password
            };
        }
    }
}