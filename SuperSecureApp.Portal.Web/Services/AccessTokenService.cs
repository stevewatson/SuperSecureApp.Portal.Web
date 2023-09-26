using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Threading.Tasks;

namespace SuperSecureApp.Portal.Web.Services
{
    public class AccessTokenService
    {
        public bool TokenAcquired = false;
        private readonly IAccessTokenProvider _accessTokenProvider;

        public AccessTokenService(IAccessTokenProvider accessTokenProvider)
        {
            _accessTokenProvider = accessTokenProvider;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var tokenResult = await _accessTokenProvider.RequestAccessToken();
            TokenAcquired = tokenResult.TryGetToken(out var accessToken);

            return accessToken.Value;
        }
    }
}
