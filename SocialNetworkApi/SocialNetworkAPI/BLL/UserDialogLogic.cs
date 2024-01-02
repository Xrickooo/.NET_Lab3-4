using Lab4.Models;
using SocialNetwork.DAL;

namespace SocialNetwork.BLL;

public class UserDialogLogic
{
    private readonly IUserRepository _userRepository;

    public UserDialogLogic(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
}