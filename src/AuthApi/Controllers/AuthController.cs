using AuthLibrary.IServices;
using AuthLibrary.Models.Request;
using AuthLibrary.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthenticateRequest request)
        {
            _logger.LogInformation($"Authentication Initiated for {request.Username}");
            AuthenticateResponse response = await _authService.AuthenticateAsync(request);
            if (!response.IsAuthenticated)
            {
                _logger.LogError($"Authentication Failed with {response.Message}");
                return Unauthorized(response);
            }
            _logger.LogInformation("Authentication Successful");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Authorize(AuthorizeRequest request)
        {
            _logger.LogInformation("Authorization Initiated");
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string accessToken = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                AuthorizeResponse response = await _authService.AuthorizeAsync(request, accessToken);
                if (!response.IsAuthorized)
                {
                    _logger.LogError($"Authorization Failed with {response.Message}");
                    return StatusCode(403, response);
                }
                _logger.LogInformation("Authorization Successful");
                return Ok(response);
            }
            _logger.LogError("Authorization Header Missing");
            return Unauthorized("Authorization Header Missing");
        }

        [HttpPost]
        public async Task<IActionResult> GetUserPermissions(UserPermissionsRequest request)
        {
            _logger.LogInformation("GetUserPermissions Initiated");
            if (Request.Headers.ContainsKey("Authorization"))
            {
                string accessToken = Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);
                UserPermissionsResponse response = await _authService.GetUserPermissionsAsync(request, accessToken);
                if (!response.IsAuthorized)
                {
                    _logger.LogError($"GetUserPermissions Failed with {response.Message}");
                    return Unauthorized(response);
                }
                _logger.LogError("GetUserPermissions completed with {@UserPermissionsResponse}", response);
                return Ok(response);
            }
            _logger.LogError("Authorization Header Missing");
            return Unauthorized("Authorization Header Missing");
        }
    }
}
