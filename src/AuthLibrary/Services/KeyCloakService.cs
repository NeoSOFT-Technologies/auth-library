using AuthLibrary.Helpers;
using AuthLibrary.IServices;
using AuthLibrary.Models.Settings;
using AuthLibrary.Models.Request;
using AuthLibrary.Models.Response;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;

namespace AuthLibrary.Services
{
    public class KeyCloakService : IAuthService
    {
        private readonly KeyCloakSettings _keyCloakSettings;
        private readonly RestClient _restClient;
        private readonly ILogger<KeyCloakService> _logger;

        public KeyCloakService(IOptions<KeyCloakSettings> keyCloakSettings, ILogger<KeyCloakService> logger)
        {
            _keyCloakSettings = keyCloakSettings.Value;
            _restClient = new RestClient(_keyCloakSettings.Host, _keyCloakSettings.TokenEndpoint);
            _logger = logger;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request)
        {
            _logger.LogInformation($"AuthenticateAsync Initiated for {request.Username}");
            KeyValuePair<string, string>[] requestBody = new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", _keyCloakSettings.ClientId),
                    new KeyValuePair<string, string>("client_secret", _keyCloakSettings.ClientSecret),
                    new KeyValuePair<string, string>("username", request.Username),
                    new KeyValuePair<string, string>("password", request.Password)
                };
            HttpResponseMessage response = await _restClient.PostAsync(requestBody);
            string responseString = await response.Content.ReadAsStringAsync();
            AuthenticateResponse responseObject = new AuthenticateResponse();
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                responseObject.IsAuthenticated = false;
                responseObject.Message = JObject.Parse(responseString)["error_description"].ToString();
                _logger.LogError($"Authentication Failed with {responseObject.Message}");
                return responseObject;
            }
            responseObject = JsonConvert.DeserializeObject<AuthenticateResponse>(responseString);
            responseObject.IsAuthenticated = true;
            _logger.LogInformation("Authentication Successful");
            return responseObject;
        }

        public async Task<AuthorizeResponse> AuthorizeAsync(AuthorizeRequest request)
        {
            _logger.LogInformation("AuthorizeAsync Initiated");
            Dictionary<string, string> headers = SetAuthorizationHeader(request.AccessToken);
            KeyValuePair<string, string>[] requestBody = new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket"),
                    new KeyValuePair<string, string>("response_mode", "decision"),
                    new KeyValuePair<string, string>("permission", $@"{request.Resource}#{request.Scope}"),
                    new KeyValuePair<string, string>("audience", _keyCloakSettings.ClientId)
                };
            HttpResponseMessage response = await _restClient.PostAsync(requestBody, headers);
            AuthorizeResponse responseObject = new AuthorizeResponse();
            string responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                responseObject.IsAuthorized = false;
                responseObject.Message = JObject.Parse(responseString)["error_description"].ToString();
                _logger.LogError($"Authorization Failed with {responseObject.Message}");
                return responseObject;
            }
            responseObject.IsAuthorized = true;
            _logger.LogInformation("Authorization Successful");
            return responseObject;
        }

        public async Task<UserPermissionsResponse> GetUserPermissionsAsync(UserPermissionsRequest request)
        {
            _logger.LogInformation("GetUserPermissionsAsync Initiated");
            Dictionary<string, string> headers = SetAuthorizationHeader(request.AccessToken);
            KeyValuePair<string, string>[] requestBody = new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket"),
                    new KeyValuePair<string, string>("response_mode", "permissions"),
                    new KeyValuePair<string, string>("audience", _keyCloakSettings.ClientId)
                };
            HttpResponseMessage response = await _restClient.PostAsync(requestBody, headers);
            string responseString = await response.Content.ReadAsStringAsync();
            UserPermissionsResponse responseObject = new UserPermissionsResponse();
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                responseObject.IsAuthorized = false;
                responseObject.Message = JObject.Parse(responseString)["error_description"].ToString();
                _logger.LogError($"GetUserPermissionsAsync Failed with {responseObject.Message}");
                return responseObject;
            }
            responseObject.Permissions = JsonConvert.DeserializeObject<List<UserPermission>>(responseString);
            responseObject.IsAuthorized = true;
            _logger.LogError("GetUserPermissionsAsync completed with {@UserPermissionsResponse}", responseObject);
            return responseObject;
        }

        private Dictionary<string, string> SetAuthorizationHeader(string accessToken)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {accessToken}" }
            };
            return headers;
        }
    }
}
