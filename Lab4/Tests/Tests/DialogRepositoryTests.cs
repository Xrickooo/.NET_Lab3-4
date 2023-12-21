using System;
using System.Data;
using System.Data.SqlClient;
using Lab4.Models;
using Moq;
using SocialNetwork.DAL;
using Xunit;

namespace SocialNetwork.Tests
{
    public class MockDialogRepositoryTests
    {
        [Fact]
        public void FindDialogId_ExistingDialog_ReturnsDialogId()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(1); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            int dialogId = repository.FindDialogId(1, 2);

            // Assert
            Assert.Equal(1, dialogId);
        }

        [Fact]
        public void FindDialogId_NonExistingDialog_ReturnsNegativeOne()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns((object)null); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            int dialogId = repository.FindDialogId(1, 2);

            // Assert
            Assert.Equal(-1, dialogId);
        }

        [Fact]
        public void FindDialogId_DatabaseError_ReturnsNegativeOne()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar())
                .Throws(new Exception("Database error"));

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            int dialogId = repository.FindDialogId(1, 2);

            // Assert
            Assert.Equal(-1, dialogId);
        }

        [Fact]
        public void CreateDialog_FriendDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.CreateDialog(1, "friendLogin");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateDialog_NotFriends_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.CreateDialog(1, "friendLogin");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateDialog_DialogAlreadyExists_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.CreateDialog(1, "friendLogin");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateDialog_DatabaseError_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery())
                .Throws(new Exception("Database error")); 
            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.CreateDialog(1, "friendLogin");

            // Assert
            Assert.False(result);
        }




        [Fact]
        public void IsDialogExists_DialogExists_ReturnsTrue()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(1); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.IsDialogExists(1, 2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDialogExists_DialogDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(0); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.IsDialogExists(1, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDialogExists_DatabaseError_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar())
                .Throws(new Exception("Database error")); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.IsDialogExists(1, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AreFriends_FriendsExist_ReturnsTrue()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(1); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.AreFriends(1, 2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AreFriends_FriendsDoNotExist_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(0); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.AreFriends(1, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AreFriends_DatabaseError_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar())
                .Throws(new Exception("Database error")); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.AreFriends(1, 2);

            // Assert
            Assert.False(result);
        }
        

        [Fact]
        public void GetDialogMessages_DialogNotFound_ReturnsNull()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(-1);

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            List<Message> messages = repository.GetDialogMessages(1, "FriendLogin");

            // Assert
            Assert.Null(messages);
        }

        [Fact]
        public void GetDialogMessages_DatabaseError_ReturnsNull()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Throws(new Exception("Database error"));

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            List<Message> messages = repository.GetDialogMessages(1, "FriendLogin");

            // Assert
            Assert.Null(messages);
        }

        [Fact]
        public void SendMessage_InvalidMessageContent_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.SendMessage(1, 2, "");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SendMessage_DatabaseError_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Throws(new Exception("Database error")); 

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.SendMessage(1, 2, "Hello, friend!");

            // Assert
            Assert.False(result);
        }
    }
}
    





