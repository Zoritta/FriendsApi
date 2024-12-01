using System.IO;
using FriendsApi.Models;
using FriendsApi.Utilities;
using Microsoft.AspNetCore.Mvc;


namespace FriendsApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class FriendsController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _path;
    private readonly List<Friend> _friends;


    public FriendsController(IWebHostEnvironment environment)
    {
        _environment = environment;
        _path = string.Concat(_environment.ContentRootPath, "/Data/friends.json");
        _friends = LoadFriends();


    }
    [HttpGet()]
    public ActionResult ListFriends()
    {
        return Ok(new { success = true, data = _friends });
    }

    [HttpGet("relation/{relationType}")]
    public ActionResult FindFriend(string relationType)
    {

        if (_friends == null || !_friends.Any())
        {
            return NotFound(new { success = false, message = "No friends found." });
        }
        var normalizedInput = relationType.Replace(" ", "").ToLower();
        var matchingFriends = _friends
            .Where(v => v.RelationType.Replace(" ", "").ToLower() == normalizedInput)
            .ToList();

        if (matchingFriends.Any())
        {
            return Ok(new { success = true, data = matchingFriends });
        }

        return NotFound(new { success = false, message = "No friends found with the specified relation type." });
    }


    [HttpGet("id/{id}")]
    public ActionResult GetFriendById(int id)
    {
        var friend = _friends.SingleOrDefault(v => v.Id == id);

        if (friend is not null)
        {
            return Ok(new { success = true, data = friend });
        }

        return NotFound(new { success = false, message = "Friend not found" });
    }
    [HttpPost()]
    public ActionResult AddFriend(Friend friend)
    {
        var newFriend = Create(friend);

        // CreatedAtAction function makes an url for FindFriend() method(first argument, second argument provides the argument that FindFriend method needs, 3rd argument is returning the new friend in the body of the http result.)
        return CreatedAtAction(nameof(FindFriend), new { id = newFriend.Id }, newFriend);
    }
    [HttpDelete("id")]
    public ActionResult DeleteFriend(int id)
    {
        Remove(id);
        return NoContent();
    }
    [HttpPut("{id}")]
    public ActionResult UpdateFriend(Friend friend, int id)
    {
        Update(id, friend);
        return NoContent();
    }
    private Friend Create(Friend friend)
    {
        friend.Id = _friends.Count + 1;
        _friends.Add(friend);
        Save(_friends);

        return friend;
    }
    private void Save(List<Friend> list)
    {
        Storage<Friend>.WriteJson(_path, list);
    }

    private void Remove(int id)
    {
        var toDelete = _friends.SingleOrDefault(Friend => Friend.Id == id);
        Friend.Remove(toDelete);
        Storage<Friend>.WriteJson(_path, _friends);
    }
    private void Update(int id, Friend toUpdateFriend)
    {
        var filteredList = _friends.FindAll(vehicle => vehicle.Id != id);
        filteredList.Add(toUpdateFriend);
        Storage<Friend>.WriteJson(_path, filteredList);
    }
    private List<Friend> LoadFriends()
    {
        return Storage<Friend>.ReadJson(_path);
    }

}

