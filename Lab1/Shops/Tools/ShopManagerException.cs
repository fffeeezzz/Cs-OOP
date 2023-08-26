namespace Shops.Tools;

public class ShopManagerException : ArgumentException
{
    public ShopManagerException(string message)
        : base(message)
    {
    }

    public ShopManagerException(string message, string paramName)
        : base(message, paramName)
    {
    }
}