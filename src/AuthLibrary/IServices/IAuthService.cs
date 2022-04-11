using AuthLibrary.Models.Request;
using AuthLibrary.Models.Response;
using System.Threading.Tasks;

namespace AuthLibrary.IServices
{
    public interface IAuthService
    {
        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request);
        Task<AuthorizeResponse> AuthorizeAsync(AuthorizeRequest request);
        Task<UserPermissionsResponse> GetUserPermissionsAsync(UserPermissionsRequest request);
    }
}
