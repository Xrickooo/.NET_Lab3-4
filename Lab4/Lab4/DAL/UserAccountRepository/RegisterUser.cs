using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SocialNetwork.DAL;

public partial class UserAccountRepository
{
    public RegistrationResult RegisterUser(string login, string firstName, string lastName, string email, string password)
    {
        if (!IsValidEmail(email) || !IsValidLogin(login) || !IsValidName(firstName) || !IsValidName(lastName) || !IsValidPassword(password))
        {
            return RegistrationResult.InvalidData;
        }
    
        if (IsEmailAlreadyRegistered(email) || IsLoginAlreadyRegistered(login))
        {
            return RegistrationResult.EmailAlreadyExists; 
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Users (Login, FirstName, LastName, Email, Password) VALUES (@Login, @FirstName, @LastName, @Email, @Password)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Login", login);
            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", HashPassword(password));

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0 ? RegistrationResult.Success : RegistrationResult.Failure;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return RegistrationResult.Failure;
            }
        }
    }
    
    public string HashPassword(string password)
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
    
    private bool IsValidLogin(string login)
    {
        return !string.IsNullOrWhiteSpace(login) && login.Length >= 3 && Regex.IsMatch(login, @"^[a-zA-Z0-9_]+$");
    }
    
    public bool IsLoginAlreadyRegistered(string login)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Login = @Login";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Login", login);

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
                return userCount > 0; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

    }


}