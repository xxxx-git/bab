namespace Shared.Config {
    public interface ITokenSettings {
        string Secret { get; set; }
        IHeaderSettings Headers { get; set; }
        IClaimSettings Claims { get; set;}
    }

    public interface IHeaderSettings {
        string Type {get; set;}
        string Algorithm {get; set;}
    }

    public interface IClaimSettings {
        string Issuer { get; set; }
        string Audience { get; set; }
        string Subject { get; set; }
        IExpiresClaimSettings Expires { get; set; }
        string User { get; set; }
    }

    public interface IExpiresClaimSettings {
        int Months {get; set;}
        int Days {get; set;}
        int Hours {get; set;}
    }
}