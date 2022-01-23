using System;
using System.Net;
using System.Threading.Tasks;
using DevCon.Code.Business.Dto;
using DevCon.Code.Business.Mappings;
using DevCon.Code.Business.Models;
using DevCon.Code.Business.Repositories;
using DevCon.Code.Common;

namespace DevCon.Code.Business.Services
{
    public interface IDevConUserService
    {
        Task<ServiceResponse<DevConUserDto>> FindUserAsync(Guid userId);
        Task<ServiceResponse<DevConUserDto>> CreateUserAsync(CreateUserModel newUser);
    }

    public class DevConUserService : IDevConUserService
    {
        private readonly IDevConUserRepository _devConUserRepository;
        private readonly ISecurePasswordService _securePasswordService;

        public DevConUserService(IDevConUserRepository devConUserRepository, ISecurePasswordService securePasswordService)
        {
            _devConUserRepository = devConUserRepository;
            _securePasswordService = securePasswordService;
        }

        public async Task<ServiceResponse<DevConUserDto>> FindUserAsync(Guid userId)
        {
            var user = await _devConUserRepository.FindUserAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return ServiceResponse<DevConUserDto>.WithError(HttpStatusCode.NotFound, ErrorMessages.UserNotFound);
            }

            return ServiceResponse<DevConUserDto>.WithResult(user.AdaptToDto());
        }

        public async Task<ServiceResponse<DevConUserDto>> CreateUserAsync(CreateUserModel newUser)
        {
            var exists = await _devConUserRepository.UserExistsAsync(newUser.EmailAddress).ConfigureAwait(false);
            if (exists)
            {
                return ServiceResponse<DevConUserDto>.WithError(HttpStatusCode.Found, ErrorMessages.UserAlreadyExists);
            }

            var passwordSalt = await _securePasswordService.GeneratePasswordSaltAsync().ConfigureAwait(false);
            var passwordHash = await _securePasswordService.HashPasswordAsync(newUser.Password, passwordSalt).ConfigureAwait(false);

            var dbUser = await _devConUserRepository.CreateUserAsync(newUser.AdaptToDto(passwordHash).AdaptToEntity(passwordSalt));
            if (dbUser == null)
            {
                return ServiceResponse<DevConUserDto>.WithError(HttpStatusCode.InternalServerError, ErrorMessages.CreateUserExecutionFailed);
            }

            return ServiceResponse<DevConUserDto>.WithResult(dbUser.AdaptToDto());
        }
    }
}