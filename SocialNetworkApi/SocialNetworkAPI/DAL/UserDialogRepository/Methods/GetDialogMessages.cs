using System.Data.SqlClient;
using Lab4.Models;

namespace SocialNetwork.DAL;

public partial class DialogRepository
{
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
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT m.MessageContent, m.SentDate, u.Login AS SenderLogin " +
                           "FROM Messages m " +
                           "JOIN Users u ON m.SenderID = u.UserID " +
                           "WHERE m.DialogID = @DialogID " +
                           "ORDER BY m.SentDate";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DialogID", dialogId);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
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

}