using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class Lesson : IEquatable<Lesson>
{
    private const int MinClassroomNumber = 1;
    private const int MinLessonNumber = 1;
    private const int MaxLessonNumber = 8;

    public Lesson(string name, string teacher, int classroomNumber, int lessonNumber)
    {
        Validate(name, teacher, classroomNumber, lessonNumber);

        Name = name;
        Teacher = teacher;
        ClassroomNumber = classroomNumber;
        LessonNumber = lessonNumber;
    }

    public string Name { get; }
    public string Teacher { get; }
    public int ClassroomNumber { get; }
    public int LessonNumber { get; }

    public bool Equals(Lesson other)
    {
        return other != null &&
               Name == other.Name &&
               Teacher == other.Teacher &&
               ClassroomNumber == other.ClassroomNumber &&
               LessonNumber == other.LessonNumber;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Lesson);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Teacher, ClassroomNumber, LessonNumber);
    }

    private void Validate(string name, string teacher, int classroomNumber, int lessonNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw IsuExtraServiceExceptionCollection.IsBlankOrNullException(nameof(name));
        }

        if (string.IsNullOrWhiteSpace(teacher))
        {
            throw IsuExtraServiceExceptionCollection.IsBlankOrNullException(nameof(teacher));
        }

        if (classroomNumber < MinClassroomNumber)
        {
            throw IsuExtraServiceExceptionCollection.IsLessException(nameof(classroomNumber));
        }

        if (lessonNumber is < MinLessonNumber or > MaxLessonNumber)
        {
            throw IsuExtraServiceExceptionCollection.InvalidRangeException(
                lessonNumber,
                MinLessonNumber,
                MaxLessonNumber,
                nameof(lessonNumber));
        }
    }
}