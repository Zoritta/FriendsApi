


namespace FriendsApi.Models;

public class Friend(string relationType) : Person 
{
    public string RelationType { get; set; } = relationType;

    internal static void Remove(Friend toDelete)
    {
        throw new NotImplementedException();
    }
}
