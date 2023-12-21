using Moq;
using System.Data;
using System.Data.SqlClient;
using Xunit;
using SocialNetwork.DAL;

namespace SocialNetwork.Tests
{
    public class UserAccountRepositoryTests
    {
        [Fact]
        public void LoginUser_InvalidPassword_ReturnsMinusOne()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(DBNull.Value); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            var result = repository.LoginUser("xrickooo", "wrongpassword");

            Assert.Equal(-1, result);
        }

        
        [Fact]
        public void LoginUser_ValidUser_ReturnsUserId()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            
            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(1); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            
            var result = repository.LoginUser("xrickooo", "123");
            
            Assert.Equal(1, result);
        }
        
        [Fact]
        public void RegisterUser_InvalidData_ReturnsInvalidData()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            
            var result = repository.RegisterUser("testUser", "Test", "User", "invalidemail", "password123");

            Assert.Equal(RegistrationResult.InvalidData, result);
        }

        
        [Fact]
        public void LoginUser_WhenExceptionOccurs_ReturnsMinusOne()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Throws(new Exception()); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            var result = repository.LoginUser("xrickooo", "wrongpassword");

            Assert.Equal(-1, result);
        }
        
        [Fact]
        public void LoginUser_WithInvalidData_ReturnsMinusOne()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(DBNull.Value); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            var result = repository.LoginUser("", ""); 

            Assert.Equal(-1, result);
        }
        
        
        [Fact]
        public void RegisterUser_WithInvalidEmail_ReturnsInvalidData()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            
            var result = repository.RegisterUser("testlogin", "Test", "User", "invalidemail", "password");
            
            Assert.Equal(RegistrationResult.InvalidData, result);
        }
        
        [Fact]
        public void RegisterUser_WithInvalidName_ReturnsInvalidData()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            
            var result = repository.RegisterUser("testlogin", "", "User", "test@gmail.com", "password");
            
            Assert.Equal(RegistrationResult.InvalidData, result);
        }
        
        [Fact]
        public void RegisterUser_WithInvalidPassword_ReturnsInvalidData()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            
            var result = repository.RegisterUser("testlogin", "Test", "User", "test@gmail.com", "p");
            
            Assert.Equal(RegistrationResult.InvalidData, result);
        }
        
        [Fact]
        public void IsLoginAlreadyRegistered_WithRegisteredLogin_ReturnsTrue()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
    
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(1); 
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            var result = repository.IsLoginAlreadyRegistered("existing_login");

            Assert.True(result);
        }

        [Fact]
        public void IsLoginAlreadyRegistered_WithNonRegisteredLogin_ReturnsFalse()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
    
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(0); 
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            var result = repository.IsLoginAlreadyRegistered("new_login");

            Assert.False(result);
        }

        [Fact]
        public void IsLoginAlreadyRegistered_WhenExceptionOccurs_ReturnsFalse()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
    
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Throws(new Exception()); 
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            var result = repository.IsLoginAlreadyRegistered("some_login");

            Assert.False(result); 
        }
        
        [Fact]
        public void IsEmailAlreadyRegistered_WithRegisteredEmail_ReturnsTrue()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);

            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(1); 
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            var result = repository.IsEmailAlreadyRegistered("existing@email.com");

            Assert.True(result);
        }

        [Fact]
        public void IsEmailAlreadyRegistered_WithNonRegisteredEmail_ReturnsFalse()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);

            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(0); 
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            var result = repository.IsEmailAlreadyRegistered("new@email.com");

            Assert.False(result);
        }

        [Fact]
        public void IsEmailAlreadyRegistered_WhenExceptionOccurs_ReturnsFalse()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);

            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Throws(new Exception()); 
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            var result = repository.IsEmailAlreadyRegistered("some@email.com");

            Assert.False(result); 
        }
        
        
        [Fact]
        public void RegisterUser_WhenExceptionOccurs_ReturnsFailure()
        {
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Throws(new Exception());

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);
            var result = repository.RegisterUser("validLogin", "FirstName", "LastName", "valid@email.com", "validPassword");

            Assert.Equal(RegistrationResult.InvalidData, result);
        }
        
        [Fact]
        public void ChangePassword_ValidData_ReturnsTrue()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.ChangePassword("test@gmail.com", "newHashedPassword");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ChangePassword_NoRowsUpdated_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(0); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.ChangePassword("nonexistent@gmail.com", "newHashedPassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ChangePassword_DatabaseError_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Throws(new Exception("Database error")); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.ChangePassword("test@gmail.com", "newHashedPassword");

            // Assert
            Assert.False(result);
        }
        
        [Fact]
        public void ValidateUserCredentials_ValidCredentials_ReturnsUserId()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(1); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            // Act
            int result = repository.ValidateUserCredentials("test@gmail.com", "hashedPassword");

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void ValidateUserCredentials_InvalidCredentials_ReturnsMinusOne()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(DBNull.Value); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            // Act
            int result = repository.ValidateUserCredentials("test@gmail.com", "wrongPassword");

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void RegisterUser_ValidData_ReturnsSuccess()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            // Act
            RegistrationResult result = repository.RegisterUser("testlogin", "John", "Doe", "test@gmail.com", "hashedPassword");

            // Assert
            Assert.Equal(RegistrationResult.Success, result);
        }

        [Fact]
        public void RegisterUser_InvalidData_ReturnsInvalidData2()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            // Act
            RegistrationResult result = repository.RegisterUser("testlogin", "", "Doe", "invalidemail", "password");

            // Assert
            Assert.Equal(RegistrationResult.InvalidData, result);
        }

        [Fact]
        public void RegisterUser_DatabaseError_ReturnsFailure()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Throws(new Exception("Database error")); 

            var repository = new MockUserAccountRepository(mockConnectionFactory.Object);

            // Act
            RegistrationResult result = repository.RegisterUser("testlogin", "John", "Doe", "test@gmail.com", "hashedPassword");

            // Assert
            Assert.Equal(RegistrationResult.Failure, result);
        }
    }
}


    
    
    




