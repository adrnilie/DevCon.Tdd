using System;
using System.Threading.Tasks;

namespace DevCon.Code.Business.Services
{
    public interface ISecurePasswordService
    {
        Task<string> GeneratePasswordSaltAsync();
        Task<string> HashPasswordAsync(string password, string salt);
    }

    public class SecurePasswordService : ISecurePasswordService
    {
        public Task<string> GeneratePasswordSaltAsync()
        {
            var salt = Guid.NewGuid().ToString("N");
            return Task.FromResult(salt);
        }

        public Task<string> HashPasswordAsync(string password, string salt)
        {
            var passwordHash = $"{password}_{salt}";
            return Task.FromResult(passwordHash);
        }
    }
}