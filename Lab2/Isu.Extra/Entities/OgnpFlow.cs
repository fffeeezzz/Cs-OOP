using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class OgnpFlow : IEquatable<OgnpFlow>
{
    private const int MaxCountOfStudents = 25;
    private readonly List<StreamStudent> _streamStudents;

    public OgnpFlow(string name, char faculty, Ognp ognp)
    {
        Validate(name, faculty, ognp);

        Name = name;
        Faculty = faculty;
        Ognp = ognp;
        _streamStudents = new List<StreamStudent>();
        Schedule = new Schedule();
    }

    public string Name { get; }
    public char Faculty { get; }
    public Ognp Ognp { get; }
    public Schedule Schedule { get; private set; }
    public IEnumerable<StreamStudent> StreamStudents => _streamStudents;

    public void AddSchedule(Schedule schedule)
    {
        Schedule = schedule ?? throw IsuExtraServiceExceptionCollection.IsNullException(nameof(schedule));
    }

    public void AddStreamStudent(StreamStudent student)
    {
        EnsureCanAddStudent(student);

        _streamStudents.Add(student);
    }

    public void RemoveStreamStudent(StreamStudent student)
    {
        EnsureCanRemoveStudent(student);

        _streamStudents.Remove(student);
    }

    public bool Equals(OgnpFlow other)
    {
        return other != null &&
               Name == other.Name &&
               Faculty == other.Faculty;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as OgnpFlow);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Faculty);
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

        if (student.StreamGroup.Faculty == Faculty)
        {
            throw IsuExtraServiceExceptionCollection.NotAllowableOgnpException(nameof(student.StreamGroup.Faculty));
        }
    }

    private void EnsureCanRemoveStudent(StreamStudent student)
    {
        if (student is null)
        {
            throw IsuExtraServiceExceptionCollection.IsNullException(nameof(student));
        }

        if (!_streamStudents.Contains(student))
        {
            throw IsuExtraServiceExceptionCollection.NotExistingException(nameof(student));
        }
    }

    private void Validate(string name, char faculty, Ognp ognp)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw IsuExtraServiceExceptionCollection.IsBlankOrNullException(nameof(name));
        }

        if (!char.IsLetter(faculty) || char.IsLower(faculty))
        {
            throw IsuExtraServiceExceptionCollection.InvalidFacultyLetter(nameof(faculty));
        }

        if (faculty != ognp.Faculty)
        {
            throw IsuExtraServiceExceptionCollection.NotEqualException(nameof(faculty));
        }

        if (ognp is null)
        {
            throw IsuExtraServiceExceptionCollection.IsNullException(nameof(ognp));
        }
    }
}