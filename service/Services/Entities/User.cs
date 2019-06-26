using bab.Shared;

internal class User : IUser
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Hierarchy { get; set; }
        public string Token { get; set; }
    }