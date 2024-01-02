using System.Data.SqlClient;

namespace SocialNetwork.DAL;

public partial class FriendRequestRepository
{
    public bool RemoveFriend(int userId, string friendLogin)
    {
        int friendId = GetUserIdByLogin(friendLogin);

        if (friendId == -1)
        {
            return false; 
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "BEGIN TRANSACTION; " +
                           "DELETE FROM Relationships WHERE UserID = @UserID AND FriendID = @FriendID; " +
                           "DELETE FROM Relationships WHERE UserID = @FriendID AND FriendID = @UserID; " +
                           "COMMIT;";

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
                Console.WriteLine("Error in RemoveFriend: " + ex.Message);
                return false;
            }
        }
    }

}