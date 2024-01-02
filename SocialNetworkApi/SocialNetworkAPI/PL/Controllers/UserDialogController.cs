using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.BLL;
using Lab4.Models;
using Microsoft.AspNetCore.Authorization;

namespace SocialNetwork.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDialogController : ControllerBase
    {
        private readonly UserDialogLogic _userLogic;

        public UserDialogController(UserDialogLogic userLogic)
        {
            _userLogic = userLogic;
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