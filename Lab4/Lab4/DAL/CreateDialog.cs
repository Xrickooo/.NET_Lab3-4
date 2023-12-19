﻿using System.Data.SqlClient;

namespace SocialNetwork.DAL;

public partial class UserRepository
{
    public bool CreateDialog(int userId, string friendLogin)
    {
        int friendId = GetUserIdByLogin(friendLogin);

        if (friendId == -1)
        {
            return false; 
        }
        
        if (!AreFriends(userId, friendId))
        {
            return false; 
        }
        
        if (IsDialogExists(userId, friendId))
        {
            return false; 
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Dialogs (UserID, FriendID) VALUES (@UserID, @FriendID);";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@FriendID", friendId);

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in CreateDialog: " + ex.Message);
                return false;
            }
        }
    }

    private bool IsDialogExists(int userId, int friendId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Dialogs WHERE (UserID = @UserID AND FriendID = @FriendID) " +
                           "OR (UserID = @FriendID AND FriendID = @UserID)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@FriendID", friendId);

            try
            {
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in IsDialogExists: " + ex.Message);
                return false;
            }
        }
    }


    private bool AreFriends(int userId, int friendId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT COUNT(*) FROM Relationships WHERE (UserID = @UserID AND FriendID = @FriendID AND Status = 1) " +
                           "OR (UserID = @FriendID AND FriendID = @UserID AND Status = 1)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@FriendID", friendId);

            try
            {
                connection.Open();
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AreFriends: " + ex.Message);
                return false;
            }
        }
    }


}