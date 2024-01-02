using System.Data.SqlClient;

namespace SocialNetwork.DAL;

public partial class UserAccountRepository
{
    public bool ChangePassword(string userEmail, string hashedNewPassword)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Users SET Password = @NewPassword WHERE Email = @Email";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NewPassword", hashedNewPassword);
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
    
    public int ValidateUserCredentials(string email, string hashedPassword)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT UserID FROM Users WHERE Email = @Email AND Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", hashedPassword);

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