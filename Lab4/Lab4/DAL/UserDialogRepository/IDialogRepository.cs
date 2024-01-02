using Lab4.Models;

namespace SocialNetwork.DAL;

public interface IDialogRepository
{
    bool SendMessage(int dialogId, int senderId, string messageContent);
    bool CreateDialog(int userId, string friendLogin);
    List<Dialog> GetUserDialogs(int userId);
    List<Message> GetDialogMessages(int userId, string friendLogin);
    int FindDialogId(int userId, int friendId);
}