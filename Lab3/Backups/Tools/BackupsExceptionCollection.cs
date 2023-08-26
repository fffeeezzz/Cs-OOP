namespace Backups.Tools;

public class BackupsExceptionCollection : ArgumentException
{
    public static BackupsException IsBlankOrNullException(string paramName)
        => new BackupsException("This value should not be blank or null.", paramName);

    public static BackupsException IsNullException(string paramName)
        => new BackupsException("This value should not be null.", paramName);

    public static BackupsException NotUniqueException(string paramName)
        => new BackupsException("This value is already used.", paramName);

    public static BackupsException FileDoesNotExistException(string paramName)
        => new BackupsException("This file does not exist.", paramName);
}