using AuthLibrary.IServices;
using AuthLibrary.Models.Request;
using AuthLibrary.Models.Response;

namespace AuthLibrary.Services
{
    public class IdentityServerService : IAuthService
    {
        public Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AuthorizeResponse> AuthorizeAsync(AuthorizeRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserPermissionsResponse> GetUserPermissionsAsync(UserPermissionsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
