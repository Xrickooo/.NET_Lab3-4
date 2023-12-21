using System.Data.SqlClient;
using Lab4.Models;

namespace SocialNetwork.DAL;
public enum FriendRequestResult
{
    Success,
    AlreadySent,
    Failure
}

public partial class FriendRequestRepository
{
    public bool SendFriendRequest(int userId, int friendId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Relationships (UserID, FriendID) VALUES (@UserID, @FriendID)";
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
    
    public bool IsFriendRequestAlreadySent(int userId, int friendId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Relationships WHERE UserID = @UserID AND FriendID = @FriendID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@FriendID", friendId);

            try
            {
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }

    public int GetUserIdByLogin(string login)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT UserID FROM Users WHERE Login = @Login";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Login", login);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
        }
    }


}