using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Lab4.Models;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork;
using Lab4.Models;
using SocialNetwork.DAL;

namespace SocialNetwork.BLL
{
    public class UserLogic 
    {
        private readonly IUserRepository _userRepository;

        public UserLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public RegistrationResult RegisterUser(string login, string firstName, string lastName, string email, string password)
        {
            return _userRepository.UserAccountRepository.RegisterUser(login, firstName, lastName, email, password);
        }
        
        public int LoginUser(string login, string password)
        {
            return _userRepository.UserAccountRepository.LoginUser(login, password);
        }
        
        public bool ChangePasswordForLoggedInUser(string userEmail, string oldPassword, string newPassword)
        {
            string hashedOldPassword = HashPassword(oldPassword);
            int userId = _userRepository.UserAccountRepository.ValidateUserCredentials(userEmail, hashedOldPassword);
            if (userId != -1)
            {
                string hashedNewPassword = HashPassword(newPassword);
                return _userRepository.UserAccountRepository.ChangePassword(userEmail, hashedNewPassword);
            }
            return false;
        }


        
        public FriendRequestResult SendFriendRequest(int userId, string friendLogin)
        {
            int friendId = _userRepository.FriendRequestRepository.GetUserIdByLogin(friendLogin);

            if (friendId == -1)
            {
                return FriendRequestResult.Failure; 
            }
    
            if (userId == friendId)
            {
                return FriendRequestResult.Failure; 
            }

            if (_userRepository.FriendRequestRepository.IsFriendRequestAlreadySent(userId, friendId))
            {
                return FriendRequestResult.AlreadySent; 
            }
            else if (_userRepository.FriendRequestRepository.SendFriendRequest(userId, friendId))
            {
                return FriendRequestResult.Success; 
            }
            else
            {
                return FriendRequestResult.Failure; 
            }
        }

        
        public bool RespondToFriendRequest(int userId, string friendLogin, bool accept)
        {
            return _userRepository.FriendRequestRepository.RespondToFriendRequest(userId, friendLogin, accept);
        }
        
        public List<FriendRequest> GetFriendRequests(int userId)
        {
            return _userRepository.FriendRequestRepository.GetFriendRequests(userId);
        }
        
        public List<string> GetFriends(int userId)
        {
            return _userRepository.FriendRequestRepository.GetFriends(userId);
        }
        
        public bool RemoveFriend(int userId, string friendLogin)
        {
            return _userRepository.FriendRequestRepository.RemoveFriend(userId, friendLogin);
        }
        
        public bool CreateDialog(int userId, string friendLogin)
        {
            return _userRepository.DialogRepository.CreateDialog(userId, friendLogin);
        }
        
        public List<Dialog> GetUserDialogs(int userId)
        {
            return _userRepository.DialogRepository.GetUserDialogs(userId);
        }
        
        public bool SendMessageToFriend(int userId, string friendLogin, string messageContent)
        {
            int friendId = _userRepository.FriendRequestRepository.GetUserIdByLogin(friendLogin);
            if (friendId == -1)
            {
                return false; 
            }

            int dialogId = _userRepository.DialogRepository.FindDialogId(userId, friendId);
            if (dialogId == -1)
            {
                return false; 
            }

            return _userRepository.DialogRepository.SendMessage(dialogId, userId, messageContent);
        }

        public List<Message> GetDialogMessages(int userId, string friendLogin)
        {
            return _userRepository.DialogRepository.GetDialogMessages(userId, friendLogin);
        }
        
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
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