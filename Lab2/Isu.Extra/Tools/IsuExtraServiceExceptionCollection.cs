namespace Isu.Extra.Tools;

public static class IsuExtraServiceExceptionCollection
{
    public static IsuExtraServiceException IsBlankOrNullException(string paramName)
        => new IsuExtraServiceException("This value should not be blank or null.", paramName);

    public static IsuExtraServiceException IsNullException(string paramName)
        => new IsuExtraServiceException("This value should not be null.", paramName);

    public static IsuExtraServiceException IsLessException(string paramName)
        => new IsuExtraServiceException("This value is too low.", paramName);

    public static IsuExtraServiceException IsHighException(string paramName)
        => new IsuExtraServiceException("This value is too high.", paramName);

    public static IsuExtraServiceException InvalidRangeException(int value, int min, int max, string paramName)
        => new IsuExtraServiceException($"This value: {value} should be between {min} and {max}.", paramName);

    public static IsuExtraServiceException NotUniqueException(string paramName)
        => new IsuExtraServiceException("This value is already used.", paramName);

    public static IsuExtraServiceException InvalidFacultyLetter(string paramName)
        => new IsuExtraServiceException("This value is already used.", paramName);

    public static IsuExtraServiceException NotAllowableOgnpException(string paramName)
        => new IsuExtraServiceException("This ognp is not allowable to you because of the same faculty.", paramName);

    public static IsuExtraServiceException NotEqualException(string paramName)
        => new IsuExtraServiceException("Value is not equal to required.", paramName);

    public static IsuExtraServiceException NotExistingException(string paramName)
        => new IsuExtraServiceException("This value does not exist.", paramName);

    public static IsuExtraServiceException IntersectionsException(string paramName)
        => new IsuExtraServiceException("Exist intersections between lessons.", paramName);

    public static IsuExtraServiceException NotExistingOgnpWithSameFlowException(string paramName)
        => new IsuExtraServiceException("Ognp does not exist with same flow.", paramName);

    public static IsuExtraServiceException NotDifferentFacultyException(string paramName)
        => new IsuExtraServiceException("Faculties should be different.", paramName);
}