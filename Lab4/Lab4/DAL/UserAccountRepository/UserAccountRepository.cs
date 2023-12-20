namespace SocialNetwork.DAL;

public partial class UserAccountRepository : IUserAccountRepository
{
    private readonly string _connectionString;

    public UserAccountRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    
}