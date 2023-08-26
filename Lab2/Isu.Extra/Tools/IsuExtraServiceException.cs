namespace Isu.Extra.Tools;

public class IsuExtraServiceException : ArgumentException
{
    public IsuExtraServiceException(string message)
        : base(message)
    {
    }

    public IsuExtraServiceException(string message, string paramName)
        : base(message, paramName)
    {
    }
}