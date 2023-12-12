using SocialNetwork.DAL;

namespace SocialNetwork.BLL
{
    public class UserLogic 
    {
        private readonly UserRepository _userRepository;

        public UserLogic(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool RegisterUser(string firstName, string lastName, string email, string password)
        {
            return _userRepository.RegisterUser(firstName, lastName, email, password);
        }
        
        public bool LoginUser(string email, string password)
        {
            return _userRepository.ValidateUser(email, password);
        }
    }
}