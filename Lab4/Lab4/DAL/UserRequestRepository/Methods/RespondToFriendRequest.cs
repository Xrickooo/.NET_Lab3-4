using System.Data.SqlClient;

namespace SocialNetwork.DAL;

public partial class FriendRequestRepository
{
    public bool RespondToFriendRequest(int userId, string friendLogin, bool accept)
    {
        int friendId = GetUserIdByLogin(friendLogin);

        if (friendId == -1)
        {
            return false; 
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query;
            if (accept)
            {
                query = "UPDATE Relationships SET Status = 1 WHERE UserID = @FriendID AND FriendID = @UserID AND Status = 0";
            }
            else
            {
                query = "DELETE FROM Relationships WHERE UserID = @FriendID AND FriendID = @UserID AND Status = 0";
            }

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@FriendID", friendId);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in RespondToFriendRequest: " + ex.Message);
                return false;
            }
        }
    }
}