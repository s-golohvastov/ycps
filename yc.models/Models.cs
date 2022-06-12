namespace yc.models
{
    public class EndpointRecord
    {
        public string? id  { get; set; }
        public string? address  { get; set; }
    }

    public class EndpointList
    {
        public EndpointRecord[] endpoints  { get; set; }
    }

    public class YandexOauthRequest
    {
        public string yandexPassportOauthToken { get; set; }
    }

    public class IAMTokenRecord
    {
        public string iamToken { get; set; }
        public DateTime expiresAt { get; set; }
    }
}