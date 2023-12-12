using Microsoft.AspNetCore.Mvc;
using SocialNetwork.BLL;
using Lab4.Models;

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
            bool isRegistered = _userLogic.RegisterUser(model.FirstName, model.LastName, model.Email, model.Password);

            if (isRegistered)
            {
                return Ok("User successfully registered");
            }
            else
            {
                return BadRequest("Failed to register user");
            }
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] UserLoginModel model)
        {
            bool isLoggedIn = _userLogic.LoginUser(model.Email, model.Password);

            if (isLoggedIn)
            {
                return Ok("User successfully logged in");
            }
            else
            {
                return BadRequest("Invalid email or password");
            }
        }
    }
}
