using System.ComponentModel.DataAnnotations;

namespace AuthLibrary.Models.Settings
{
    public class KeyCloakSettings
    {
        [Required]
        public string Host { get; set; }
        [Required]
        public string TokenEndpoint { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ClientSecret { get; set; }
    }
}
