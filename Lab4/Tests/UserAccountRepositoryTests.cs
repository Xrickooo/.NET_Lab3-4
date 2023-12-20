using Xunit;
using Moq;
using SocialNetwork.DAL;
using System.Data;
using Lab4;
using SocialNetwork.BLL;

namespace SocialNetwork.Tests
{
    public class UserAccountRepositoryTests
    {
        public const string _connectionstring  = @"Data Source=.\SQLExpress;Initial Catalog=SocialNetwork;Integrated Security=True;";
        
        [Fact]
        public void RegisterUser_EmailAlreadyExists_ReturnsEmailAlreadyExists()
        {
            var repository = new UserAccountRepository(_connectionstring);
            var result = repository.RegisterUser("validLogin", "John", "Doe", "user@gmail.com", "Password123!");
            Assert.Equal(RegistrationResult.EmailAlreadyExists, result);
        }
        
        
        [Theory]
        [InlineData("invalidemail", "validLogin", "John", "Doe", "Password123!")]
        [InlineData("user@gmail.com", "vl", "John", "Doe", "Password123!")]
        [InlineData("user@gmail.com", "validLogin", "", "Doe", "Password123!")]
        [InlineData("user@gmail.com", "validLogin", "John", "", "Password123!")]
        public void RegisterUser_InvalidData_ReturnsInvalidData(string email, string login, string firstName,
            string lastName, string password)
        {
            // Arrange
            var repository =
                new UserAccountRepository(
                    _connectionstring);

            // Act
            var result = repository.RegisterUser(login, firstName, lastName, email, password);

            // Assert
            Assert.Equal(RegistrationResult.InvalidData, result);
        }
        
    }
    

    
    

}



