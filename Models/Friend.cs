

namespace FriendsApi.Models;

public class Friend(string relationType) : Person 
{
    public string RelationType { get; set; } = relationType;
}
