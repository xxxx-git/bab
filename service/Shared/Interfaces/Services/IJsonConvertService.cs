
namespace Shared {

    public interface IJsonCovnertService 
    {
        string Serialize(object json);
        object Deserialize(string jsonString);
    }
}