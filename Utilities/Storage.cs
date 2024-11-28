using System.Text.Json;
using FriendsApi.Models;

namespace FriendsApi.Utilities;

public class Storage<T>
{
     public  static List<Friend>ReadJson(string path)
     {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var json = File.ReadAllText(path);
        var result = JsonSerializer.Deserialize<List<Friend>>(json, options);
        return result;
     }
}