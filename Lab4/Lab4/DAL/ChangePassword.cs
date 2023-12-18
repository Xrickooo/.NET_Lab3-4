using System.Data.SqlClient;
using Lab4.Models;

namespace SocialNetwork.DAL;

public partial class UserRepository
{
    public bool ChangePassword(string userEmail, string newPassword)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Users SET Password = @NewPassword WHERE Email = @Email";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NewPassword", HashPassword(newPassword));
            command.Parameters.AddWithValue("@Email", userEmail);

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