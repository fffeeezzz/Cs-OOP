using Isu.Entities;
using Isu.Extra.Tools;
using Isu.Models;

namespace Isu.Extra.Entities;

public class StreamGroup : Group, IEquatable<StreamGroup>
{
    private readonly List<StreamStudent> _streamStudents;

    public StreamGroup(GroupName groupName)
        : base(groupName)
    {
        Faculty = Convert.ToChar(groupName.Name[0]);
        Schedule = new Schedule();
        _streamStudents = new List<StreamStudent>();
        Schedule = new Schedule();
    }

    public char Faculty { get; }
    public Schedule Schedule { get; private set; }
    public new IEnumerable<StreamStudent> Students => _streamStudents;

    public void AddSchedule(Schedule schedule)
    {
        Schedule = schedule ?? throw IsuExtraServiceExceptionCollection.IsNullException(nameof(schedule));
    }

    public void AddStreamStudent(StreamStudent student)
    {
        EnsureCanAddStudent(student);

        _streamStudents.Add(student);
    }

    public bool Equals(StreamGroup other)
    {
        return other != null && GroupName.Name == other.GroupName.Name;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as StreamGroup);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GroupName.Name);
    }

    private void EnsureCanAddStudent(StreamStudent student)
    {
        if (student is null)
        {
            throw IsuExtraServiceExceptionCollection.IsNullException(nameof(student));
        }

        if (_streamStudents.Count >= MaxCountOfStudents)
        {
            throw IsuExtraServiceExceptionCollection.IsHighException(nameof(_streamStudents.Count));
        }

        if (_streamStudents.Contains(student))
        {
            throw IsuExtraServiceExceptionCollection.NotUniqueException(nameof(student));
        }
    }
}