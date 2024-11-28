using FriendsApi.Models;
using FriendsApi.Utilities;
using Microsoft.AspNetCore.Mvc;


namespace FriendsApi.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class FriendsController : ControllerBase
{   
        private readonly IWebHostEnvironment _environment;
    public FriendsController(IWebHostEnvironment environment)
    {
           _environment = environment;
        
    }
    [HttpGet()]
    public ActionResult ListFriends()
     {
        var path = string.Concat(_environment.ContentRootPath, "/Data/friends.json");
        var friends = Storage<Friend>.ReadJson(path);
        return Ok(new { success = true, data = friends});
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

}

