namespace Backups.Tools;

public class BackupsException : ArgumentException
{
    public BackupsException(string message)
        : base(message)
    {
    }

    public BackupsException(string message, string paramName)
        : base(message, paramName)
    {
    }
}