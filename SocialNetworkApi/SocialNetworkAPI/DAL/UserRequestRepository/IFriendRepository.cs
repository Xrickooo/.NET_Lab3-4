using Lab4.Models;

namespace SocialNetwork.DAL;

public interface IFriendRequestRepository
{
    bool SendFriendRequest(int userId, int friendId);
    bool RespondToFriendRequest(int userId, string friendLogin, bool accept);
    bool RemoveFriend(int userId, string friendLogin);
    List<FriendRequest> GetFriendRequests(int userId);
    List<string> GetFriends(int userId);
    
    bool IsFriendRequestAlreadySent(int userId, int friendId);

    public int GetUserIdByLogin(string login);
    
}