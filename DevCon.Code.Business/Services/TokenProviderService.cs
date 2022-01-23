using System;
using System.Net;
using System.Threading.Tasks;

namespace DevCon.Code.Business.Services
{
    public interface ITokenProviderService
    {
        Task<string> GenerateConfirmEmailTokenAsync();
    }

    public class TokenProviderService : ITokenProviderService
    {
        public Task<string> GenerateConfirmEmailTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");
            return Task.FromResult(WebUtility.UrlEncode(token));
        }
    }
}