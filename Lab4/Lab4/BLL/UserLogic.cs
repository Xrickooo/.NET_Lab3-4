using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lab4.Models;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.DAL;
using Lab4.Models;

namespace SocialNetwork.BLL
{
    public class UserLogic 
    {
        private readonly UserRepository _userRepository;

        public UserLogic(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserRepository.RegistrationResult RegisterUser(string login, string firstName, string lastName, string email, string password)
        {
            return _userRepository.RegisterUser(login, firstName, lastName, email, password);
        }
        
        public int LoginUser(string login, string password)
        {
            return _userRepository.LoginUser(login, password);
        }

        
        public bool ChangePasswordForLoggedInUser(string userEmail, string oldPassword, string newPassword)
        {
            int userId = _userRepository.LoginUser(userEmail, oldPassword);
            if (userId != -1) 
            {
                return _userRepository.ChangePassword(userEmail, newPassword);
            }
            return false; 
        }

        
        public UserRepository.FriendRequestResult SendFriendRequest(int userId, string friendLogin)
        {
            int friendId = _userRepository.GetUserIdByLogin(friendLogin);

            if (friendId == -1)
            {
                return UserRepository.FriendRequestResult.Failure; // Користувача не знайдено
            }
    
            if (userId == friendId)
            {
                return UserRepository.FriendRequestResult.Failure; // Неможливо відправити запит самому собі
            }

            if (_userRepository.IsFriendRequestAlreadySent(userId, friendId))
            {
                return UserRepository.FriendRequestResult.AlreadySent; // Запит вже відправлено
            }
            else if (_userRepository.SendFriendRequest(userId, friendId))
            {
                return UserRepository.FriendRequestResult.Success; // Запит успішно відправлено
            }
            else
            {
                return UserRepository.FriendRequestResult.Failure; // Запит не вдалося відправити
            }
        }

        
        public bool RespondToFriendRequest(int userId, int friendId, bool accept)
        {
            return _userRepository.RespondToFriendRequest(userId, friendId, accept);
        }
        
        public List<FriendRequest> GetFriendRequests(int userId)
        {
            return _userRepository.GetFriendRequests(userId);
        }

        public string GenerateJwtToken(string email, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678876543211234567887654321"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken("your_issuer",
                "your_audience",
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}