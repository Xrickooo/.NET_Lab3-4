using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.DAL;

namespace SocialNetwork.BLL
{
    public class UserAccountLogic 
    {
        private readonly IUserRepository _userRepository;

        public UserAccountLogic(IUserRepository userRepository)
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