namespace Shared.Config {
    public interface ITokenHttpSettings
    {
        string Header {get;set;}
        string Cookie {get;set;}
        bool HttpOnlyAccessCookie {get;set;}
    }
}