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

    [HttpGet("{relationType}")]
    public ActionResult FindFriend(string relationType)
     {
        var path = string.Concat(_environment.ContentRootPath, "/Data/friends.json");
        var friends = Storage<Friend>.ReadJson(path);
        var friend = friends.FirstOrDefault(v => v.RelationType == "Best Friend");
        
        if (friend is not null)
        {
            return Ok(new { success = true, data = friend });
        }

        return NotFound();
    }
}

