using AuthLibrary.IServices;
using AuthLibrary.Models.Request;
using AuthLibrary.Models.Response;
using System;
using System.Threading.Tasks;

namespace AuthLibrary.Services
{
    public class OctaService : IAuthService
    {
        public Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<AuthorizeResponse> AuthorizeAsync(AuthorizeRequest request, string accessToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserPermissionsResponse> GetUserPermissionsAsync(UserPermissionsRequest request, string accessToken)
        {
            throw new NotImplementedException();
        }
    }
}
