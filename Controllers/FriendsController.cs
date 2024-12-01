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
    private string path;

    public FriendsController(IWebHostEnvironment environment)
    {
        _environment = environment;

    }
    [HttpGet()]
    public ActionResult ListFriends()
    {
        var path = string.Concat(_environment.ContentRootPath, "/Data/friends.json");
        var friends = Storage<Friend>.ReadJson(path);
        return Ok(new { success = true, data = friends });
    }

    [HttpGet("relation/{relationType}")]
    public ActionResult FindFriend(string relationType)
    {
        var path = string.Concat(_environment.ContentRootPath, "/Data/friends.json");
        var friends = Storage<Friend>.ReadJson(path);

        if (friends == null || !friends.Any())
        {
            return NotFound(new { success = false, message = "No friends found." });
        }
        var normalizedInput = relationType.Replace(" ", "").ToLower();
        var matchingFriends = friends
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
        var path = string.Concat(_environment.ContentRootPath, "/Data/friends.json");
        var friends = Storage<Friend>.ReadJson(path);
        var friend = friends.SingleOrDefault(v => v.Id == id);

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
        var path = string.Concat(_environment.ContentRootPath, "/Data/friends.json");
        var friends = Storage<Friend>.ReadJson(path);
        friend.Id = friends.Count + 1;
        friends.Add(friend);
        Save(friends);

        return friend;
    }
    private void Save(List<Friend> list)
    {
        Storage<Friend>.WriteJson(path = string.Concat(_environment.ContentRootPath, "/Data/friends.json"), list);
    }

    private void Remove(int id)
    {
        var path = string.Concat(_environment.ContentRootPath, "/Data/friends.json");
        var friends = Storage<Friend>.ReadJson(path);
        var toDelete = friends.SingleOrDefault(Friend => Friend.Id == id);
        Friend.Remove(toDelete);
        Storage<Friend>.WriteJson(path, friends);
    }
    private void Update(int id, Friend toUpdateFriend)
    {
        var path = string.Concat(_environment.ContentRootPath, "/Data/friends.json");
        var friends = Storage<Friend>.ReadJson(path);
        var filteredList = friends.FindAll(vehicle => vehicle.Id != id);
        filteredList.Add(toUpdateFriend);
        Storage<Friend>.WriteJson(path, filteredList);
    }

}

