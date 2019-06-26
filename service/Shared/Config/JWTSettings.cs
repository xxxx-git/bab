namespace Shared.Config {

    interface IJWTSettings
    {
        string Secret {get;set;}
    }
    public class JWTSettings : IJWTSettings
    {
        public string Secret { get; set;}
    }
}