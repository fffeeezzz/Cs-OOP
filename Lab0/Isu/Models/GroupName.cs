using Isu.Tools;

namespace Isu.Models;

public class GroupName
{
    private const int AllowableLenOfGroupName = 5;
    private const int ValidEducationDegreeNumber = 3;

    public GroupName(string name)
    {
        Validate(name);

        Name = name;
        CourseNumber = new CourseNumber(int.Parse(name.Substring(2, 1)));
    }

    public string Name { get; }
    public CourseNumber CourseNumber { get; }

    private static void Validate(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            throw IsuServiceValidationExceptionCollection.IsBlankOrNullException(nameof(groupName));
        }

        if (groupName.Length != AllowableLenOfGroupName)
        {
            throw IsuServiceValidationExceptionCollection.InvalidLenOfGroupNameException(
                groupName,
                AllowableLenOfGroupName,
                nameof(groupName));
        }

        if (!char.IsLetter(groupName[0]) || !char.IsUpper(groupName[0]))
        {
            throw IsuServiceValidationExceptionCollection.InvalidFacultyLetterException(
                groupName[0],
                nameof(groupName));
        }

        foreach (char c in groupName.AsSpan(1))
        {
            if (!char.IsDigit(c))
            {
                throw IsuServiceValidationExceptionCollection.IsDigitAtGroupNameException(nameof(groupName));
            }
        }

        int educationDegree = int.Parse(groupName.AsSpan(1, 1));
        if (educationDegree != ValidEducationDegreeNumber)
        {
            throw IsuServiceValidationExceptionCollection.EducationDegreeException(
                educationDegree,
                nameof(groupName));
        }
    }
}