namespace SocialNetwork.DAL;

public interface IUserAccountRepository
{
    RegistrationResult RegisterUser(string login, string firstName, string lastName, string email, string password);
    int LoginUser(string login, string password);
    bool ChangePassword(string userEmail, string hashedNewPassword);
    int ValidateUserCredentials(string userEmail, string hashedOldPassword);
    
}