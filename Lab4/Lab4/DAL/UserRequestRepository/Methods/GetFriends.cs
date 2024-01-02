using System.Data.SqlClient;

namespace SocialNetwork.DAL;

public partial class FriendRequestRepository
{
    public List<string> GetFriends(int userId)
    {
        List<string> friends = new List<string>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT u.Login FROM Users u JOIN Relationships r ON u.UserID = r.FriendID WHERE r.UserID = @UserID AND r.Status = 1 " +
                           "UNION " +
                           "SELECT u.Login FROM Users u JOIN Relationships r ON u.UserID = r.UserID WHERE r.FriendID = @UserID AND r.Status = 1";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        friends.Add(reader.GetString(reader.GetOrdinal("Login")));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetFriends: " + ex.Message);
            }
        }

        return friends;
    }

}