using Isu.Extra.Entities;
using Isu.Extra.Tools;
using Isu.Models;

namespace Isu.Extra.Services;

public class IsuExtraService : IIsuExtraService
{
    private readonly List<Ognp> _ognps;
    private readonly List<StreamGroup> _streamGroups;
    private readonly List<StreamStudent> _streamStudents;
    private int _nextIsuNumber = 100000;

    public IsuExtraService()
    {
        _ognps = new List<Ognp>();
        _streamGroups = new List<StreamGroup>();
        _streamStudents = new List<StreamStudent>();
    }

    public Ognp AddOgnp(string name, char faculty)
    {
        var ognp = new Ognp(name, faculty);
        if (IsExistOgnp(ognp)) throw IsuExtraServiceExceptionCollection.NotUniqueException(nameof(ognp));

        _ognps.Add(ognp);

        return ognp;
    }

    public StreamGroup AddStreamGroup(string groupName)
    {
        var streamGroup = new StreamGroup(new GroupName(groupName));
        if (IsExistStreamGroup(streamGroup))
            throw IsuExtraServiceExceptionCollection.NotUniqueException(nameof(streamGroup));

        _streamGroups.Add(streamGroup);

        return streamGroup;
    }

    public OgnpFlow AddOgnpFlowToOgnp(string name, char faculty, Ognp ognp)
    {
        var ognpFlow = new OgnpFlow(name, faculty, ognp);

        Ognp savedOgnp = _ognps.SingleOrDefault(ognp => Equals(ognp, ognpFlow.Ognp));
        if (savedOgnp is null) throw IsuExtraServiceExceptionCollection.IsNullException(nameof(savedOgnp));

        savedOgnp.AddOgnpFlow(ognpFlow);

        return ognpFlow;
    }

    public StreamStudent AddStudentToStreamGroup(StreamGroup streamGroup, string name)
    {
        if (streamGroup is null) throw IsuExtraServiceExceptionCollection.IsNullException(nameof(streamGroup));
        if (!IsExistStreamGroup(streamGroup))
            throw IsuExtraServiceExceptionCollection.NotExistingException(nameof(streamGroup));

        var student = new StreamStudent(_nextIsuNumber++, name, streamGroup);
        if (IsExistStreamStudent(student)) throw IsuExtraServiceExceptionCollection.NotUniqueException(nameof(student));

        streamGroup.AddStreamStudent(student);
        _streamStudents.Add(student);

        return student;
    }

    public StreamStudent AddStudentToOgnpFlow(StreamStudent streamStudent, OgnpFlow ognpFlow)
    {
        EnsureStreamStudentAndOgnpFlowAreExist(streamStudent, ognpFlow);

        streamStudent.AddOgnpFlow(ognpFlow);
        ognpFlow.AddStreamStudent(streamStudent);

        return streamStudent;
    }

    public void DropStudentOutFromOgnpFlow(StreamStudent streamStudent, OgnpFlow ognpFlow)
    {
        EnsureStreamStudentAndOgnpFlowAreExist(streamStudent, ognpFlow);

        streamStudent.DropOutFromOgnp(ognpFlow);
        ognpFlow.RemoveStreamStudent(streamStudent);
    }

    public IEnumerable<StreamGroup> GetStreamGroupsByCourseNumber(CourseNumber courseNumber)
    {
        if (courseNumber is null) throw IsuExtraServiceExceptionCollection.IsNullException(nameof(courseNumber));

        return _streamGroups.Where(x => x.GroupName.CourseNumber.Number == courseNumber.Number);
    }

    public IEnumerable<StreamStudent> GetStreamStudentsFromOgnpFlow(OgnpFlow ognpFlow)
    {
        if (ognpFlow is null) throw IsuExtraServiceExceptionCollection.IsNullException(nameof(ognpFlow));

        Ognp savedOgnp = _ognps.SingleOrDefault(ognp => ognp.OgnpFlows.Contains(ognpFlow));
        if (savedOgnp is null)
            throw IsuExtraServiceExceptionCollection.NotExistingOgnpWithSameFlowException(nameof(ognpFlow));

        return ognpFlow.StreamStudents;
    }

    public IEnumerable<StreamStudent> GetStreamStudentsWithoutOgnpFromStreamGroup(StreamGroup group)
    {
        if (group is null) throw IsuExtraServiceExceptionCollection.IsNullException(nameof(group));
        if (!IsExistStreamGroup(group)) throw IsuExtraServiceExceptionCollection.NotExistingException(nameof(group));

        return group.Students.Where(student => !student.OgnpFlows.Any());
    }

    private bool IsExistOgnp(Ognp ognp) => _ognps.Contains(ognp);
    private bool IsExistStreamGroup(StreamGroup streamGroup) => _streamGroups.Contains(streamGroup);
    private bool IsExistStreamStudent(StreamStudent streamStudent) => _streamStudents.Contains(streamStudent);

    private void EnsureStreamStudentAndOgnpFlowAreExist(StreamStudent streamStudent, OgnpFlow ognpFlow)
    {
        if (streamStudent is null) throw IsuExtraServiceExceptionCollection.IsNullException(nameof(streamStudent));
        if (ognpFlow is null) throw IsuExtraServiceExceptionCollection.IsNullException(nameof(ognpFlow));
        if (!IsExistStreamStudent(streamStudent))
            throw IsuExtraServiceExceptionCollection.NotExistingException(nameof(streamStudent));
        if (!IsExistOgnp(ognpFlow.Ognp))
            throw IsuExtraServiceExceptionCollection.NotExistingException(nameof(ognpFlow.Ognp));
    }
}