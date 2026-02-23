namespace Common.Exception;

public class NotFoundException<T> : System.Exception where T : class
{
    public NotFoundException(string message) : base(message)
    {
    }
}