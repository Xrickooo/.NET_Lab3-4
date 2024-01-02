using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.BLL;
using Microsoft.AspNetCore.Authorization;
using SocialNetwork.DAL;

namespace SocialNetwork.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRequestController : ControllerBase
    {
        private readonly UserRequestLogic _userLogic;

        public UserRequestController(UserRequestLogic userLogic)
        {
            _userLogic = userLogic;
        }
        
        [Authorize]
        [HttpPost("sendRequest")]
        public IActionResult SendFriendRequest(string friendLogin)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated");
            }

            if (!int.TryParse(userId, out int currentUserId))
            {
                return BadRequest("Invalid user ID format");
            }

            
            var result = _userLogic.SendFriendRequest(currentUserId, friendLogin);
            switch (result)
            {
                case FriendRequestResult.Success:
                    return Ok("Friend request sent successfully");

                case FriendRequestResult.AlreadySent:
                    return BadRequest("Friend request already sent");

                case FriendRequestResult.Failure:
                default:
                    return BadRequest("Failed to send friend request");
            }
        }
        
        [Authorize]
        [HttpPut("relationships/respondToRequest")]
        public IActionResult RespondToFriendRequest(string friendLogin, bool accept)
        {
            var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User is not authenticated");
            }

            if (!int.TryParse(userIdString, out int currentUserId))
            {
                return BadRequest("Invalid user ID format");
            }

            bool result = _userLogic.RespondToFriendRequest(currentUserId, friendLogin, accept);
            if (result)
            {
                return Ok($"Friend request {(accept ? "accepted" : "declined")} successfully");
            }
            else
            {
                return BadRequest("Failed to respond to friend request");
            }
        }
        
        [Authorize]
        [HttpGet("friendRequests")]
        public IActionResult GetFriendRequests()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated");
            }

            if (!int.TryParse(userId, out int currentUserId))
            {
                return BadRequest("Invalid user ID format");
            }

            var requests = _userLogic.GetFriendRequests(currentUserId);
            return Ok(requests);
        }
        
        [Authorize]
        [HttpGet("friends")]
        public IActionResult GetFriends()
        {
            var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User is not authenticated");
            }

            if (!int.TryParse(userIdString, out int currentUserId))
            {
                return BadRequest("Invalid user ID format");
            }

            var friends = _userLogic.GetFriends(currentUserId);
            return Ok(friends);
        }
        
        [Authorize]
        [HttpDelete("friends/remove")]
        public IActionResult RemoveFriend(string friendLogin)
        {
            var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User is not authenticated");
            }

            if (!int.TryParse(userIdString, out int currentUserId))
            {
                return BadRequest("Invalid user ID format");
            }

            bool result = _userLogic.RemoveFriend(currentUserId, friendLogin);
            if (result)
            {
                return Ok("Friend removed successfully");
            }
            else
            {
                return BadRequest("Failed to remove friend");
            }
        }
    }
}