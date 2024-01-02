using System.Data;
using System.Data.SqlClient;
using Lab4.Models;
using Moq;
using SocialNetwork.DAL;

namespace SocialNetwork.Tests;

public class FriendRequestRepositoryTests
{

        [Fact]
        public void RemoveFriend_FriendDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.RemoveFriend(1, "friendLogin");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveFriend_DatabaseError_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Throws(new Exception("Database error")); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.RemoveFriend(1, "friendLogin");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetFriendRequests_ValidUserId_NoRequests_ReturnsEmptyList()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            var mockReader = new Mock<IDataReader>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(mockReader.Object);
            mockReader.Setup(reader => reader.Read()).Returns(false);

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            List<FriendRequest> requests = repository.GetFriendRequests(1);

            // Assert
            Assert.Empty(requests);
        }
    

        [Fact]
        public void RemoveFriend_FriendNotFound_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(0);

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.RemoveFriend(1, "FriendLogin");

            // Assert
            Assert.False(result);
        }

                

        [Fact]
        public void GetFriends_InvalidUserId_ReturnsEmptyList()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            var mockReader = new Mock<IDataReader>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(mockReader.Object);
            mockReader.Setup(r => r.Read()).Returns(false); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            List<string> friends = repository.GetFriends(1);

            // Assert
            Assert.NotNull(friends);
            Assert.Empty(friends); 
        }
        
        [Fact]
        public void RespondToFriendRequest_AcceptRequest_InvalidData_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(0); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.RespondToFriendRequest(1, "FriendLogin", accept: true);

            // Assert
            Assert.False(result);
        }

        

        [Fact]
        public void RespondToFriendRequest_DeclineRequest_InvalidData_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(0); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.RespondToFriendRequest(1, "FriendLogin", accept: false);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RespondToFriendRequest_InvalidFriend_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.RespondToFriendRequest(1, "NonExistentFriend", accept: true);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RespondToFriendRequest_DatabaseError_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Throws(new Exception("Database error")); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.RespondToFriendRequest(1, "FriendLogin", accept: true);

            // Assert
            Assert.False(result);
        }
    
        [Fact]
        public void SendFriendRequest_ValidData_ReturnsTrue()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.SendFriendRequest(1, 2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SendFriendRequest_InvalidData_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(0); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.SendFriendRequest(1, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SendFriendRequest_DatabaseError_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Throws(new Exception("Database error")); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.SendFriendRequest(1, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsFriendRequestAlreadySent_RequestExists_ReturnsTrue()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            var readerMock = new Mock<IDataReader>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(1); 

            mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(readerMock.Object);
            readerMock.Setup(r => r.Read()).Returns(true);
            readerMock.Setup(r => r.GetInt32(0)).Returns(1); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.IsFriendRequestAlreadySent(1, 2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsFriendRequestAlreadySent_RequestDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            var readerMock = new Mock<IDataReader>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Returns(0); 

            mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(readerMock.Object);
            readerMock.Setup(r => r.Read()).Returns(false); 

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.IsFriendRequestAlreadySent(1, 2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsFriendRequestAlreadySent_DatabaseError_ReturnsFalse()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteScalar()).Throws(new Exception("Database error"));

            var repository = new MockFriendRequestRepository(mockConnectionFactory.Object);

            // Act
            bool result = repository.IsFriendRequestAlreadySent(1, 2);

            // Assert
            Assert.False(result);
        }
        
        [Fact]
        public void GetUserDialogs_ValidData_ReturnsDialogs()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            var mockReader = new Mock<IDataReader>();
            
            mockReader.SetupSequence(reader => reader.Read())
                .Returns(true) 
                .Returns(true); 
            mockReader.Setup(reader => reader.GetInt32(It.IsAny<int>())).Returns(1); 
            mockReader.Setup(reader => reader.GetString(It.IsAny<int>())).Returns("FriendLogin"); 
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.Setup(cmd => cmd.Parameters.Add(It.IsAny<SqlParameter>())).Verifiable();
            mockCommand.Setup(cmd => cmd.ExecuteReader()).Returns(mockReader.Object);
            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);

            var repository = new MockDialogRepository(mockConnectionFactory.Object);

            // Act
            List<Dialog> dialogs = repository.GetUserDialogs(1);

            // Assert
            Assert.NotNull(dialogs);
            Assert.Equal(2, dialogs.Count); 
        }

        
    }

