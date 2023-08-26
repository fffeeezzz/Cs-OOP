using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class Schedule
{
    private const int MaxLessonsCount = 8;

    private readonly List<Lesson> _lessons;

    public Schedule()
    {
        _lessons = new List<Lesson>();
    }

    public IEnumerable<Lesson> Lessons => _lessons;

    public Schedule AddLesson(Lesson lesson)
    {
        EnsureCanAddLesson(lesson);

        _lessons.Add(lesson);

        return this;
    }

    public void EnsureNoIntersections(IEnumerable<Lesson> lessons)
    {
        if (lessons.Any(lesson => _lessons.Any(l => l.LessonNumber == lesson.LessonNumber)))
        {
            throw IsuExtraServiceExceptionCollection.IntersectionsException(nameof(lessons));
        }
    }

    private void EnsureCanAddLesson(Lesson lesson)
    {
        if (lesson is null)
        {
            throw IsuExtraServiceExceptionCollection.IsNullException(nameof(lesson));
        }

        if (_lessons.Count >= MaxLessonsCount)
        {
            throw IsuExtraServiceExceptionCollection.IsHighException(nameof(_lessons.Count));
        }
    }
}