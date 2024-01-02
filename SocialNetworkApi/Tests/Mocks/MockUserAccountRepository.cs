using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SocialNetwork.DAL;


public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}

public partial class MockUserAccountRepository : IUserAccountRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public MockUserAccountRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    
    public int LoginUser(string login, string password)
    {
        var hashedPassword = HashPassword(password); // Переконайтеся, що у вас є цей метод

        using (var connection = _connectionFactory.CreateConnection())
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT UserID FROM Users WHERE Login = @Login AND Password = @Password";
            command.Parameters.Add(new SqlParameter("@Login", login));
            command.Parameters.Add(new SqlParameter("@Password", hashedPassword));

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
    
    public RegistrationResult RegisterUser(string login, string firstName, string lastName, string email, string password)
{
    if (!IsValidEmail(email) || !IsValidLogin(login) || !IsValidName(firstName) || !IsValidName(lastName) || !IsValidPassword(password))
    {
        return RegistrationResult.InvalidData;
    }

    if (IsEmailAlreadyRegistered(email) || IsLoginAlreadyRegistered(login))
    {
        return RegistrationResult.EmailAlreadyExists; // Переконайтеся, що у вас є такий статус
    }

    using (var connection = _connectionFactory.CreateConnection())
    {
        var command = connection.CreateCommand();
        command.CommandText = "INSERT INTO Users (Login, FirstName, LastName, Email, Password) VALUES (@Login, @FirstName, @LastName, @Email, @Password)";
        command.Parameters.Add(new SqlParameter("@Login", login));
        command.Parameters.Add(new SqlParameter("@FirstName", firstName));
        command.Parameters.Add(new SqlParameter("@LastName", lastName));
        command.Parameters.Add(new SqlParameter("@Email", email));
        command.Parameters.Add(new SqlParameter("@Password", HashPassword(password)));

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
        using (var connection = _connectionFactory.CreateConnection())
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Users WHERE Login = @Login";
            command.Parameters.Add(new SqlParameter("@Login", login));

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
        using (var connection = _connectionFactory.CreateConnection())
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            command.Parameters.Add(new SqlParameter("@Email", email));

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
    
    public bool ChangePassword(string userEmail, string hashedNewPassword)
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            var command = connection.CreateCommand();
            command.CommandText = "UPDATE Users SET Password = @NewPassword WHERE Email = @Email";
            command.Parameters.Add(new SqlParameter("@NewPassword", hashedNewPassword));
            command.Parameters.Add(new SqlParameter("@Email", userEmail));

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
        using (var connection = _connectionFactory.CreateConnection())
        {
            var command = connection.CreateCommand();
            command.CommandText = "SELECT UserID FROM Users WHERE Email = @Email AND Password = @Password";
            command.Parameters.Add(new SqlParameter("@Email", email));
            command.Parameters.Add(new SqlParameter("@Password", hashedPassword));

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