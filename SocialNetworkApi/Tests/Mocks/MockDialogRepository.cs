using System.Data.SqlClient;
using Lab4.Models;

namespace SocialNetwork.DAL;

public partial class MockDialogRepository : IDialogRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public MockDialogRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public bool CreateDialog(int userId, string friendLogin)
    {
        int friendId = GetUserIdByLogin(friendLogin);

        if (friendId == -1)
        {
            return false;
        }

        if (!AreFriends(userId, friendId))
        {
            return false;
        }

        if (IsDialogExists(userId, friendId))
        {
            return false;
        }

        using (var connection = _connectionFactory.CreateConnection())
        {
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Dialogs (UserID, FriendID) VALUES (@UserID, @FriendID);";
            command.Parameters.Add(new SqlParameter("@UserID", userId));
            command.Parameters.Add(new SqlParameter("@FriendID", friendId));

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreateDialog: " + ex.Message);
                return false;
            }
        }
    }

    public bool IsDialogExists(int userId, int friendId)
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT COUNT(*) FROM Dialogs WHERE (UserID = @UserID AND FriendID = @FriendID) " +
                           "OR (UserID = @FriendID AND FriendID = @UserID)";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@UserID", userId));
            command.Parameters.Add(new SqlParameter("@FriendID", friendId));

            try
            {
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in IsDialogExists: " + ex.Message);
                return false;
            }
        }
    }



    public bool AreFriends(int userId, int friendId)
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT COUNT(*) FROM Relationships WHERE (UserID = @UserID AND FriendID = @FriendID AND Status = 1) " +
                           "OR (UserID = @FriendID AND FriendID = @UserID AND Status = 1)";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@UserID", userId));
            command.Parameters.Add(new SqlParameter("@FriendID", friendId));

            try
            {
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AreFriends: " + ex.Message);
                return false;
            }
        }
    }

    
    public int GetUserIdByLogin(string login)
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT UserID FROM Users WHERE Login = @Login";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@Login", login));

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetUserIdByLogin: " + ex.Message);
                return -1;
            }
        }
    }

    
    public List<Message> GetDialogMessages(int userId, string friendLogin)
    {
        int friendId = GetUserIdByLogin(friendLogin);
        if (friendId == -1)
        {
            return null; 
        }

        int dialogId = FindDialogId(userId, friendId);
        if (dialogId == -1)
        {
            return null; 
        }

        List<Message> messages = new List<Message>();
        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT m.MessageContent, m.SentDate, u.Login AS SenderLogin " +
                           "FROM Messages m " +
                           "JOIN Users u ON m.SenderID = u.UserID " +
                           "WHERE m.DialogID = @DialogID " +
                           "ORDER BY m.SentDate";

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@DialogID", dialogId));

            try
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        messages.Add(new Message
                        {
                            Content = reader.GetString(reader.GetOrdinal("MessageContent")),
                            SentDate = reader.GetDateTime(reader.GetOrdinal("SentDate")),
                            SenderLogin = reader.GetString(reader.GetOrdinal("SenderLogin"))
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetDialogMessages: " + ex.Message);
            }
        }

        return messages;
    }

    
    public List<Dialog> GetUserDialogs(int userId)
    {
        List<Dialog> dialogs = new List<Dialog>();

        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT d.DialogID, u.UserID, u.Login FROM Dialogs d " +
                           "JOIN Users u ON d.FriendID = u.UserID OR d.UserID = u.UserID " +
                           "WHERE (d.UserID = @UserID OR d.FriendID = @UserID) AND u.UserID != @UserID";

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@UserID", userId));

            try
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dialogs.Add(new Dialog
                        {
                            DialogId = reader.GetInt32(reader.GetOrdinal("DialogID")),
                            FriendId = reader.GetInt32(reader.GetOrdinal("UserID")),
                            FriendLogin = reader.GetString(reader.GetOrdinal("Login"))
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetUserDialogs: " + ex.Message);
            }
        }

        return dialogs;
    }

    
    public bool SendMessage(int dialogId, int senderId, string messageContent)
    {
        if (string.IsNullOrWhiteSpace(messageContent))
        {
            return false;
        }

        int receiverId = GetReceiverIdForDialog(dialogId, senderId);
        if (receiverId == -1)
        {
            return false;
        }

        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "INSERT INTO Messages (DialogID, SenderID, ReceiverID, MessageContent, SentDate) VALUES (@DialogID, @SenderID, @ReceiverID, @MessageContent, GETDATE());";

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@DialogID", dialogId));
            command.Parameters.Add(new SqlParameter("@SenderID", senderId));
            command.Parameters.Add(new SqlParameter("@ReceiverID", receiverId));
            command.Parameters.Add(new SqlParameter("@MessageContent", messageContent));

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SendMessage: " + ex.Message);
                return false;
            }
        }
    }


    
    private int GetReceiverIdForDialog(int dialogId, int senderId)
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT (CASE WHEN UserID = @SenderID THEN FriendID ELSE UserID END) AS ReceiverID FROM Dialogs WHERE DialogID = @DialogID";

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@DialogID", dialogId));
            command.Parameters.Add(new SqlParameter("@SenderID", senderId));

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetReceiverIdForDialog: " + ex.Message);
                return -1;
            }
        }
    }
    
    
    
    public int FindDialogId(int userId, int friendId)
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT DialogID FROM Dialogs WHERE (UserID = @UserID AND FriendID = @FriendID) " +
                           "OR (UserID = @FriendID AND FriendID = @UserID)";

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@UserID", userId));
            command.Parameters.Add(new SqlParameter("@FriendID", friendId));

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FindDialogId: " + ex.Message);
                return -1;
            }
        }
    }
    

    
}