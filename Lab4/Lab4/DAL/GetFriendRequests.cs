using System.Data.SqlClient;
using Lab4.Models;

namespace SocialNetwork.DAL;

public partial class UserRepository
{
    public List<FriendRequest> GetFriendRequests(int userId)
    {
        List<FriendRequest> requests = new List<FriendRequest>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT u.Login FROM Relationships r JOIN Users u ON r.UserID = u.UserID WHERE r.FriendID = @FriendID AND r.Status = 0";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FriendID", userId);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        requests.Add(new FriendRequest
                        {
                            UserLogin = reader.GetString(reader.GetOrdinal("Login")),
                            FriendId = userId, 
                            Status = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        return requests;
    }
}