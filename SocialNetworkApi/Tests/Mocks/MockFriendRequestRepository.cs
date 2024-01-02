using System.Data.SqlClient;
using Lab4.Models;

namespace SocialNetwork.DAL;

public partial class MockFriendRequestRepository : IFriendRequestRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public MockFriendRequestRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public List<FriendRequest> GetFriendRequests(int userId)
    {
        List<FriendRequest> requests = new List<FriendRequest>();

        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT u.Login FROM Relationships r JOIN Users u ON r.UserID = u.UserID WHERE r.FriendID = @FriendID AND r.Status = 0";
        
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@FriendID", userId));

            try
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
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

    
    public List<string> GetFriends(int userId)
    {
        List<string> friends = new List<string>();

        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT u.Login FROM Users u JOIN Relationships r ON u.UserID = r.FriendID WHERE r.UserID = @UserID AND r.Status = 1 " +
                           "UNION " +
                           "SELECT u.Login FROM Users u JOIN Relationships r ON u.UserID = r.UserID WHERE r.FriendID = @UserID AND r.Status = 1";
        
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@UserID", userId));

            try
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
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
    
    public bool RemoveFriend(int userId, string friendLogin)
    {
        int friendId = GetUserIdByLogin(friendLogin);

        if (friendId == -1)
        {
            return false; 
        }

        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "BEGIN TRANSACTION; " +
                           "DELETE FROM Relationships WHERE UserID = @UserID AND FriendID = @FriendID; " +
                           "DELETE FROM Relationships WHERE UserID = @FriendID AND FriendID = @UserID; " +
                           "COMMIT;";

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@UserID", userId));
            command.Parameters.Add(new SqlParameter("@FriendID", friendId));

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
    
    public bool RespondToFriendRequest(int userId, string friendLogin, bool accept)
    {
        int friendId = GetUserIdByLogin(friendLogin);

        if (friendId == -1)
        {
            return false; 
        }

        using (var connection = _connectionFactory.CreateConnection())
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

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@UserID", userId));
            command.Parameters.Add(new SqlParameter("@FriendID", friendId));

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

    
    public bool SendFriendRequest(int userId, int friendId)
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "INSERT INTO Relationships (UserID, FriendID) VALUES (@UserID, @FriendID)";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@UserID", userId));
            command.Parameters.Add(new SqlParameter("@FriendID", friendId));

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
        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT COUNT(*) FROM Relationships WHERE UserID = @UserID AND FriendID = @FriendID";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@UserID", userId));
            command.Parameters.Add(new SqlParameter("@FriendID", friendId));

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
        using (var connection = _connectionFactory.CreateConnection())
        {
            string query = "SELECT UserID FROM Users WHERE Login = @Login";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(new SqlParameter("@Login", login));

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