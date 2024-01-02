namespace SocialNetwork.DAL
{
    public class UserRepository : IUserRepository
    {

        public UserRepository( 
            IUserAccountRepository userAccountRepo, 
            IFriendRequestRepository friendRequestRepo, 
            IDialogRepository dialogRepo)
        {
            UserAccountRepository = userAccountRepo;
            FriendRequestRepository = friendRequestRepo;
            DialogRepository = dialogRepo;
        }

        public IUserAccountRepository UserAccountRepository { get; private set; }
        public IFriendRequestRepository FriendRequestRepository { get; private set; }
        public IDialogRepository DialogRepository { get; private set; }
    }

    public enum RegistrationResult
    {
        Success,
        EmailAlreadyExists,
        InvalidData,
        Failure
    }

}
