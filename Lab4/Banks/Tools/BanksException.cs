namespace Banks.Tools;

public class BanksException : ArgumentException
{
    public BanksException(string message)
        : base(message)
    {
    }

    public BanksException(string message, string paramName)
        : base(message, paramName)
    {
    }
}