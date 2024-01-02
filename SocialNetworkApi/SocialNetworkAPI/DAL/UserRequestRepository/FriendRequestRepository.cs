namespace SocialNetwork.DAL;

public partial class FriendRequestRepository : IFriendRequestRepository
{
    private readonly string _connectionString;

    public FriendRequestRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
}