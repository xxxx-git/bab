namespace bab.Shared 
{
    public interface IUser 
    {
        string Id { get; set;}
        string DisplayName { get; set;}
        string Hierarchy { get; set;}
        string Token { get; set; }
    }
}