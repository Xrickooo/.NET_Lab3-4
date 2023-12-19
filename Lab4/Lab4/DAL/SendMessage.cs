using System.Data.SqlClient;

namespace SocialNetwork.DAL;

public partial class UserRepository
{
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

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Messages (DialogID, SenderID, ReceiverID, MessageContent, SentDate) VALUES (@DialogID, @SenderID, @ReceiverID, @MessageContent, GETDATE());";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DialogID", dialogId);
            command.Parameters.AddWithValue("@SenderID", senderId);
            command.Parameters.AddWithValue("@ReceiverID", receiverId);
            command.Parameters.AddWithValue("@MessageContent", messageContent);

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
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT (CASE WHEN UserID = @SenderID THEN FriendID ELSE UserID END) AS ReceiverID FROM Dialogs WHERE DialogID = @DialogID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DialogID", dialogId);
            command.Parameters.AddWithValue("@SenderID", senderId);

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
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT DialogID FROM Dialogs WHERE (UserID = @UserID AND FriendID = @FriendID) " +
                           "OR (UserID = @FriendID AND FriendID = @UserID)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@FriendID", friendId);

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