namespace AuthLibrary.Models.Response
{
    public class AuthenticateResponse
    {
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public int refresh_expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public string notBeforePolicy { get; set; }
        public string session_state { get; set; }
        public string scope { get; set; }
    }
}
