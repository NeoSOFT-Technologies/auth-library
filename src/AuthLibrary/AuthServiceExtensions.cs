using AuthLibrary.IServices;
using AuthLibrary.Models.Settings;
using AuthLibrary.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthLibrary
{
    public static class AuthServiceExtensions
    {
        public static void AddKeyCloakServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KeyCloakSettings>(configuration.GetSection("KeyCloakSettings"));
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
