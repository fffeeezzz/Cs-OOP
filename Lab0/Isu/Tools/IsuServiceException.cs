namespace Isu.Tools;

public class IsuServiceException : ArgumentException
{
    public IsuServiceException(string message)
        : base(message)
    {
    }

    public IsuServiceException(string message, string paramName)
        : base(message, paramName)
    {
    }
}