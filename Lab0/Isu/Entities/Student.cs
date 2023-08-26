using Isu.Tools;

namespace Isu.Entities;

public class Student
{
    private const int MinValidIsuNumber = 100000;
    private const int MaxValidIsuNumber = 999999;

    public Student(int isuNumber, string name)
    {
        EnsureIsuNumberAndStudentNameAreValid(isuNumber, name);

        IsuNumber = isuNumber;
        Name = name;
    }

    public int IsuNumber { get; }
    public string Name { get; }
    internal Group Group { get; set; }

    public Group GetGroup() => Group;

    private static void EnsureIsuNumberAndStudentNameAreValid(int isuNumber, string studentName)
    {
        if (isuNumber is < MinValidIsuNumber or > MaxValidIsuNumber)
        {
            throw IsuServiceValidationExceptionCollection.NotInRangeStudentIsuNumberException(
                isuNumber,
                MinValidIsuNumber,
                MaxValidIsuNumber,
                nameof(isuNumber));
        }

        if (string.IsNullOrWhiteSpace(studentName))
        {
            throw IsuServiceValidationExceptionCollection.IsBlankOrNullException(nameof(studentName));
        }
    }
}