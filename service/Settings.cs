using Shared.Config;

namespace bab {

    public class Settings : ITokenSettings
    {
        public IHeaderSettings Headers { get; set; } = new HeaderSettings();
        public IClaimSettings Claims { get; set; } = new ClaimsSettings();
        public string Secret { get; set;}
    }

    public class ClaimsSettings : IClaimSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public IExpiresClaimSettings Expires { get; set; } = new ExpiresClaimSettings();
        public string User { get; set; }
    }

    public class ExpiresClaimSettings : IExpiresClaimSettings
    {
        public int Months { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }
    }

    public class HeaderSettings : IHeaderSettings
    {
        public string Type { get; set; }
        public string Algorithm { get; set; }
    }
}