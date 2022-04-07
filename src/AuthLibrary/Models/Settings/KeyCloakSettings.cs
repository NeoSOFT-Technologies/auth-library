namespace AuthLibrary.Models.Settings
{
    public class KeyCloakSettings
    {
        public string Host { get; set; }
        public string TokenEndpoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
