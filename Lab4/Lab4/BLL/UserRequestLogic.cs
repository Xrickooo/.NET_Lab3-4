using Lab4.Models;
using SocialNetwork.DAL;

namespace SocialNetwork.BLL;

public class UserRequestLogic
{
    private readonly IUserRepository _userRepository;

    public UserRequestLogic(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
}