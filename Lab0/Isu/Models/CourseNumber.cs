using Isu.Tools;

namespace Isu.Models;

public class CourseNumber
{
    private const int MinCourseNumber = 1;
    private const int MaxCourseNumber = 4;

    public CourseNumber(int number)
    {
        EnsureValidCourseNumber(number);

        Number = number;
    }

    public int Number { get; }

    private static void EnsureValidCourseNumber(int courseNumber)
    {
        if (courseNumber is < MinCourseNumber or > MaxCourseNumber)
        {
            throw IsuServiceValidationExceptionCollection.NotInRangeCourseNumberException(
                courseNumber,
                MinCourseNumber,
                MaxCourseNumber,
                nameof(courseNumber));
        }
    }
}