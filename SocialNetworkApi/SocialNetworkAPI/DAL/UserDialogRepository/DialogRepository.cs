namespace SocialNetwork.DAL;

public partial class DialogRepository : IDialogRepository
{
    private readonly string _connectionString;

    public DialogRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
}