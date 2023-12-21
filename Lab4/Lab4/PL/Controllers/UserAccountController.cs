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
    public class UserAccountController : ControllerBase
    {
        private readonly UserAccountLogic _userLogic;

        public UserAccountController(UserAccountLogic userLogic)
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
    }
}
