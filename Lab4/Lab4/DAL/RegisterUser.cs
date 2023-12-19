using System.Data.SqlClient;

namespace SocialNetwork.DAL;

public partial class UserRepository
{
    public enum RegistrationResult
    {
        Success,
        EmailAlreadyExists,
        InvalidData,
        Failure
    }

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


}