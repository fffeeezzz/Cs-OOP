namespace Shops.Tools;

public class ShopManagerExceptionCollection
{
    public static ShopManagerException IsBlankOrNullException(string paramName)
        => new ShopManagerException("This value should not be blank or null.", paramName);

    public static ShopManagerException IsNullException(string paramName)
        => new ShopManagerException("This value should not be null.", paramName);

    public static ShopManagerException IsLessException(string paramName)
        => new ShopManagerException("This value is too low.", paramName);

    public static ShopManagerException IsHighException(string paramName)
        => new ShopManagerException("This value is too high.", paramName);

    public static ShopManagerException NotUniqueException(string paramName)
        => new ShopManagerException("This value is not unique.", paramName);

    public static ShopManagerException ContainsKeyException(string paramName)
        => new ShopManagerException("This key does not exist.", paramName);

    public static ShopManagerException ContainsValueException(string paramName)
        => new ShopManagerException("This value does not exist.", paramName);
}