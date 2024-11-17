namespace WebApplication1.Model
{
    public class FacebookClientModel
    {
        public string FacebookClientId { get; set; }
        public string FacebookSecretKey { get; set; }
    }

    public class FacebookTokenDebugResponse
    {
        public TokenData data { get; set; }
    }

    public class TokenData
    {
        public string app_id { get; set; }
        public string type { get; set; }
        public string application { get; set; }
        public long data_access_expires_at { get; set; }
        public long expires_at { get; set; }
        public bool is_valid { get; set; }
        public long issued_at { get; set; }
        public string[] scopes { get; set; }
        public string user_id { get; set; }
    }
}
