namespace AuthLibrary.Models.Request
{
    public class AuthorizeRequest
    {
        public string Resource { get; set; }
        public string Scope { get; set; }
    }
}
