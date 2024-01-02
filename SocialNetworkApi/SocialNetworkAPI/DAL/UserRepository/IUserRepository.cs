namespace SocialNetwork.DAL;

public interface IUserRepository
{
    IUserAccountRepository UserAccountRepository { get; }
    IFriendRequestRepository FriendRequestRepository { get; }
    IDialogRepository DialogRepository { get; }
}