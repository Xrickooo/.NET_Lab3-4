using System.Data.SqlClient;
using Lab4.Models;

namespace SocialNetwork.DAL;

public partial class UserRepository
{
    public List<Dialog> GetUserDialogs(int userId)
    {
        List<Dialog> dialogs = new List<Dialog>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT d.DialogID, u.UserID, u.Login FROM Dialogs d " +
                           "JOIN Users u ON d.FriendID = u.UserID OR d.UserID = u.UserID " +
                           "WHERE (d.UserID = @UserID OR d.FriendID = @UserID) AND u.UserID != @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
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

}