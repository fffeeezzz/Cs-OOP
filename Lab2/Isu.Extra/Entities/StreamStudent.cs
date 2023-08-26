using Isu.Entities;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class StreamStudent : Student, IEquatable<StreamStudent>
{
    private const int MaxOgnpsPerStudentCount = 2;
    private readonly List<OgnpFlow> _ognpFlows;

    public StreamStudent(int isuNumber, string name, StreamGroup streamGroup)
        : base(isuNumber, name)
    {
        StreamGroup = streamGroup ?? throw IsuExtraServiceExceptionCollection.IsNullException(nameof(streamGroup));
        _ognpFlows = new List<OgnpFlow>();
    }

    public StreamGroup StreamGroup { get; }
    public IEnumerable<OgnpFlow> OgnpFlows => _ognpFlows;

    public void AddOgnpFlow(OgnpFlow ognpFlow)
    {
        EnsureCanRegisterStudentToOgnp(ognpFlow);

        _ognpFlows.Add(ognpFlow);
    }

    public void DropOutFromOgnp(OgnpFlow ognpFlow)
    {
        if (ognpFlow is null)
        {
            throw IsuExtraServiceExceptionCollection.IsNullException(nameof(ognpFlow));
        }

        if (!_ognpFlows.Contains(ognpFlow))
        {
            throw IsuExtraServiceExceptionCollection.NotExistingException(nameof(ognpFlow));
        }

        _ognpFlows.Remove(ognpFlow);
    }

    public bool Equals(StreamStudent other)
    {
        return other != null && IsuNumber == other.IsuNumber;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as StreamStudent);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_ognpFlows, StreamGroup);
    }

    private void EnsureCanRegisterStudentToOgnp(OgnpFlow ognpFlow)
    {
        if (ognpFlow is null)
        {
            throw IsuExtraServiceExceptionCollection.IsNullException(nameof(ognpFlow));
        }

        StreamGroup.Schedule.EnsureNoIntersections(ognpFlow.Schedule.Lessons);

        if (_ognpFlows.Count >= MaxOgnpsPerStudentCount)
        {
            throw IsuExtraServiceExceptionCollection.IsHighException(nameof(_ognpFlows.Count));
        }

        if (StreamGroup.Faculty == ognpFlow.Faculty)
        {
            throw IsuExtraServiceExceptionCollection.NotDifferentFacultyException(nameof(ognpFlow.Faculty));
        }
    }
}