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
                case UserRepository.RegistrationResult.Success:
                    return Ok("User successfully registered");

                case UserRepository.RegistrationResult.EmailAlreadyExists:
                    return BadRequest("Email or login is already registered");

                case UserRepository.RegistrationResult.InvalidData:
                    return BadRequest("Invalid registration data");

                case UserRepository.RegistrationResult.Failure:
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
        public IActionResult ChangePassword(string email, string oldPassword, string newPassword)
        {
            bool isPasswordChanged = _userLogic.ChangePasswordForLoggedInUser(email, oldPassword, newPassword);

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
                case UserRepository.FriendRequestResult.Success:
                    return Ok("Friend request sent successfully");

                case UserRepository.FriendRequestResult.AlreadySent:
                    return BadRequest("Friend request already sent");

                case UserRepository.FriendRequestResult.Failure:
                default:
                    return BadRequest("Failed to send friend request");
            }
        }

        
        [Authorize]
        [HttpPut("relationships/respondToRequest")]
        public IActionResult RespondToFriendRequest(int userId, bool accept)
        {
            var userId2 = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId2))
            {
                return Unauthorized("User is not authenticated");
            }

            if (!int.TryParse(userId2, out int currentUserId))
            {
                return BadRequest("Invalid user ID format");
            }

            bool result = _userLogic.RespondToFriendRequest(currentUserId, userId, accept);
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




    }
}
