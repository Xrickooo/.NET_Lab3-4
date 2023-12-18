using System.Data.SqlClient;

namespace SocialNetwork.DAL;

public partial class UserRepository
{
    public bool RespondToFriendRequest(int userId, int friendId, bool accept)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query;
            if (accept)
            {
                // Прийняття запиту
                query = "BEGIN TRANSACTION; " +
                        "UPDATE Relationships SET Status = 1 WHERE UserID = @FriendID AND FriendID = @UserID AND Status = 0; " +
                        "INSERT INTO Relationships (UserID, FriendID, Status) VALUES (@UserID, @FriendID, 1); " +
                        "COMMIT;";
            }
            else
            {
                // Відхилення запиту
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
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }


    
}