using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.BLL;
using Lab4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.DAL;

namespace SocialNetwork.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserLogic _userLogic;

        public UserController(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] UserRegistrationModel model)
        {
            var result = _userLogic.RegisterUser(model.Login, model.FirstName, model.LastName, model.Email, model.Password);

            switch (result)
            {
                case RegistrationResult.Success:
                    return Ok("User successfully registered");

                case RegistrationResult.EmailAlreadyExists:
                    return BadRequest("Email or login is already registered");

                case RegistrationResult.InvalidData:
                    return BadRequest("Invalid registration data");

                case RegistrationResult.Failure:
                default:
                    return BadRequest("Failed to register user");
            }
        }
        
        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] UserLoginModel model)
        {
            int userId = _userLogic.LoginUser(model.Login, model.Password);

            if (userId != -1)
            {
                var token = _userLogic.GenerateJwtToken(model.Login, userId);
                return Ok(new { Token = token });
            }
            else
            {
                return BadRequest(new { Message = "Invalid login or password" });
            }
        }
        
        [HttpPut("changePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordModel model)
        {
            bool isPasswordChanged = _userLogic.ChangePasswordForLoggedInUser(model.Email, model.OldPassword, model.NewPassword);

            if (isPasswordChanged)
            {
                return Ok("Password successfully changed");
            }
            else
            {
                return BadRequest("Failed to change password");
            }
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
        
        [Authorize]
        [HttpPost("dialogs/create")]
        public IActionResult CreateDialog(string friendLogin)
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

            bool result = _userLogic.CreateDialog(currentUserId, friendLogin);
            if (result)
            {
                return Ok("Dialog created successfully");
            }
            else
            {
                return BadRequest("Failed to create dialog");
            }
        }
        
        [Authorize]
        [HttpGet("dialogs")]
        public IActionResult GetUserDialogs()
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

            var dialogs = _userLogic.GetUserDialogs(currentUserId);
            return Ok(dialogs);
        }
        
        [Authorize]
        [HttpPost("messages/send")]
        public IActionResult SendMessageToFriend(string friendLogin, [FromBody] MessageModel model)
        {
            var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int senderId))
            {
                return Unauthorized("User is not authenticated or invalid user ID");
            }

            bool result = _userLogic.SendMessageToFriend(senderId, friendLogin, model.Content);
            if (result)
            {
                return Ok("Message sent successfully");
            }
            else
            {
                return BadRequest("Failed to send message");
            }
        }
        
        [Authorize]
        [HttpGet("dialogs/messages")]
        public IActionResult GetDialogMessages(string friendLogin)
        {
            var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("User is not authenticated or invalid user ID");
            }

            var messages = _userLogic.GetDialogMessages(userId, friendLogin);
            if (messages == null)
            {
                return BadRequest("Dialog not found or friend not found");
            }

            return Ok(messages);
        }
    }
}
