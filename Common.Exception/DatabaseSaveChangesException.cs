namespace Common.Exception;

public class DatabaseSaveChangesException : System.Exception
{
    public DatabaseSaveChangesException(string message) : base(message)
    {
    }
}