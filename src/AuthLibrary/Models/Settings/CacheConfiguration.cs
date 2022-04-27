namespace AuthLibrary.Models.Settings
{
    public class CacheConfiguration
    {
        public int AbsoluteExpirationInMinutes { get; set; }
        public int SlidingExpirationInMinutes { get; set; }
    }
}
