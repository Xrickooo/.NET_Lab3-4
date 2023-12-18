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
            string query = "SELECT UserID, FriendID, Status FROM Relationships WHERE FriendID = @FriendID AND Status = 0";
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
                            UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                            FriendId = userId,
                            Status = reader.GetBoolean(reader.GetOrdinal("Status"))
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