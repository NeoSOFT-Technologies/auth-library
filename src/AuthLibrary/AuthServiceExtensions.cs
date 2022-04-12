using AuthLibrary.IServices;
using AuthLibrary.Models.Settings;
using AuthLibrary.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AuthLibrary
{
    public static class AuthServiceExtensions
    {
        public static void AddKeyCloakServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<KeyCloakSettings>(configuration.GetSection("KeyCloakSettings"))
                .PostConfigure<KeyCloakSettings>(settings =>
                {
                    string[] configErrors = settings.ValidationErrors().ToArray();
                    if (configErrors.Any())
                    {
                        string aggrErrors = string.Join(",", configErrors);
                        int count = configErrors.Length;
                        string configType = settings.GetType().Name;
                        throw new ValidationException(
                            $"Found {count} configuration error(s) in {configType}: {aggrErrors}");
                    }
                });
            services.AddTransient<IAuthService, KeyCloakService>();
        }
        public static void AddIdentityServerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityServerSettings>(configuration.GetSection("IdentityServerSettings"));
            services.AddTransient<IAuthService, IdentityServerService>();
        }
        public static void AddOctaServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OctaSettings>(configuration.GetSection("OctaSettings"));
            services.AddTransient<IAuthService, OctaService>();
        }
    }
}
