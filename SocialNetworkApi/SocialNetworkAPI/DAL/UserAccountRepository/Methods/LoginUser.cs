using System.Data.SqlClient;

namespace SocialNetwork.DAL;

public partial class UserAccountRepository 
{
    public int LoginUser(string login, string password)
    {
        var hashedPassword = HashPassword(password);

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT UserID FROM Users WHERE Login = @Login AND Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Login", login);
            command.Parameters.AddWithValue("@Password", hashedPassword);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return -1; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1; 
            }
        }
    }
    
    
}