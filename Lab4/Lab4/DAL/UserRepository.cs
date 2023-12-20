using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Lab4.Models;

namespace SocialNetwork.DAL
{
    public enum RegistrationResult
    {
        Success,
        EmailAlreadyExists,
        InvalidData,
        Failure
    }
    
    public interface IUserRepository
    {
        IUserAccountRepository UserAccountRepository { get; }
        IFriendRequestRepository FriendRequestRepository { get; }
        IDialogRepository DialogRepository { get; }
    }
    
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString, 
            IUserAccountRepository userAccountRepo, 
            IFriendRequestRepository friendRequestRepo, 
            IDialogRepository dialogRepo)
        {
            _connectionString = connectionString;
            UserAccountRepository = userAccountRepo;
            FriendRequestRepository = friendRequestRepo;
            DialogRepository = dialogRepo;
        }

        public IUserAccountRepository UserAccountRepository { get; private set; }
        public IFriendRequestRepository FriendRequestRepository { get; private set; }
        public IDialogRepository DialogRepository { get; private set; }
    }


}
