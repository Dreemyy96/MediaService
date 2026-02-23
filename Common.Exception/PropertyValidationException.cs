namespace Common.Exception;

public class PropertyValidationException : System.Exception
{
    public PropertyValidationException(string message) : base(message)
    {
    }
}