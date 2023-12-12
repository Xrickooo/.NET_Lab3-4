using System.Data.SqlClient;

namespace SocialNetwork.DAL;

public partial class UserRepository
{
    public bool RegisterUser(string firstName, string lastName, string email, string password)
    {
        if (!IsValidEmail(email) || !IsValidName(firstName) || !IsValidName(lastName) || !IsValidPassword(password))
        {
            Console.WriteLine("Invalid input data");
            return false;
        }
            
        if (IsEmailAlreadyRegistered(email))
        {
            Console.WriteLine("Email is already registered");
            return false;
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query =
                "INSERT INTO Users (FirstName, LastName, Email, Password) VALUES (@FirstName, @LastName, @Email, @Password)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", HashPassword(password));

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