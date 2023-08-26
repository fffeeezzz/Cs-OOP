using Isu.Entities;

namespace Isu.Tools;

public static class IsuServiceValidationExceptionCollection
{
    public static IsuServiceException IsBlankOrNullException(string paramName)
        => new IsuServiceException("This value should not be blank or null.", paramName);

    public static IsuServiceException IsNullException(string paramName)
        => new IsuServiceException("This value should not be null.", paramName);

    public static IsuServiceException ToManyElementsException(int count, string paramName)
        => new IsuServiceException(
            $"To many elements for this collection. Collection should have {count} or less elements.",
            paramName);

    public static IsuServiceException InvalidLenOfGroupNameException(
        string value,
        int allowableLength,
        string paramName)
        => new IsuServiceException(
            $"Invalid value for group name: {value}. This value have len: {value.Length}, " +
            $"but should have len: {allowableLength}.",
            paramName);

    public static IsuServiceException InvalidFacultyLetterException(char letter, string paramName)
        => new IsuServiceException(
            $"Invalid value for faculty letter at group name: {letter}." +
            "This value should be a letter in upper case",
            paramName);

    public static IsuServiceException IsDigitAtGroupNameException(string paramName)
        => new IsuServiceException("Each value from index 1 to index 4 should be a digit.", paramName);

    public static IsuServiceException EducationDegreeException(int value, string paramName)
        => new IsuServiceException(
            $"Invalid value for education degree at group name: {value}." +
            "This value should be equal to 3 for undergraduate education degree.",
            paramName);

    public static IsuServiceException NotInRangeCourseNumberException(int number, int min, int max, string paramName)
        => new IsuServiceException(
            $"Invalid value for course number: {number}. This value should be between {min} and {max}.", paramName);

    public static IsuServiceException NotInRangeStudentIsuNumberException(
        int number, int min, int max, string paramName)
        => new IsuServiceException(
            $"Invalid value for isu number at student: {number}. This value should be between {min} and {max}.",
            paramName);

    public static IsuServiceException NotUniqueStudentException(Student student, string paramName)
        => new IsuServiceException(
            $"Student {student.Name} with isu number {student.IsuNumber} is already exist.",
            paramName);

    public static IsuServiceException NotExistingStudentException(Student student, string paramName)
        => new IsuServiceException(
            $"Student {student.Name} with isu number {student.IsuNumber} does not exist.",
            paramName);

    public static IsuServiceException NotExistingStudentByIsuNumberException(int isuNumber, string paramName)
        => new IsuServiceException($"Student with isu number {isuNumber} does not exist.", paramName);

    public static IsuServiceException NotUniqueGroupException(Group group, string paramName)
        => new IsuServiceException($"Group {group.GroupName} is already exist.", paramName);

    public static IsuServiceException NotExistingGroupException(Group group, string paramName)
        => new IsuServiceException($"Group {group.GroupName} does not exist.", paramName);
}