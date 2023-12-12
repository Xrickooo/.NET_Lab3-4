using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SocialNetwork.DAL;

public partial class UserRepository
{
    public bool ValidateUser(string email, string password)
    {
        var hashedPassword = HashPassword(password);

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", hashedPassword);

            try
            {
                connection.Open();
                int userCount = (int)command.ExecuteScalar();
                return userCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }


    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[\w.+\-]+@gmail\.com$");
    }

    private bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    private bool IsValidPassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Length >= 2;
    }

    public bool IsEmailAlreadyRegistered(string email)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);

            try
            {
                connection.Open();
                int userCount = (int)command.ExecuteScalar();
                return userCount > 0; // Возвращает true, если почта уже зарегистрирована
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

    }
}