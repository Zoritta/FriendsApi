using System.Text.Json;
using FriendsApi.Models;

namespace FriendsApi.Utilities;

public class Storage<T>
{
     public  static List<T>ReadJson(string path)
     {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var json = File.ReadAllText(path);
        var result = JsonSerializer.Deserialize<List<T>>(json, options);
        return result;
     }

    internal static void WriteJson(object value, List<Friend> list)
    {
        throw new NotImplementedException();
    }
}
