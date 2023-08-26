using Isu.Extra.Entities;
using Isu.Extra.Services;
using Isu.Extra.Tools;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraTest
{
    private readonly IIsuExtraService _isuExtraService = new IsuExtraService();

    [Fact]
    public void AddOgnp_OgnpIsAlreadyExist()
    {
        _isuExtraService.AddOgnp("Кибербез", 'K');

        Assert.Throws<IsuExtraServiceException>(() => _isuExtraService.AddOgnp("Кибербез", 'K'));
    }

    [Fact]
    public void RemoveStreamStudentFromOgnpFlow_StreamStudentDoesNotBelongToOgnpFlow()
    {
        Ognp ognp = _isuExtraService.AddOgnp("Методы ПО", 'V');
        StreamGroup group = _isuExtraService.AddStreamGroup("М3211");
        StreamStudent student = _isuExtraService.AddStudentToStreamGroup(group, "Student 1");
        OgnpFlow ognpFlow = _isuExtraService.AddOgnpFlowToOgnp("V 1.1", 'V', ognp);
        _isuExtraService.AddStudentToOgnpFlow(student, ognpFlow);
        int countOfStudents = _isuExtraService.GetStreamStudentsFromOgnpFlow(ognpFlow).Count();

        _isuExtraService.DropStudentOutFromOgnpFlow(student, ognpFlow);
        int countOfStudentsAfterRemove = _isuExtraService.GetStreamStudentsFromOgnpFlow(ognpFlow).Count();

        Assert.Equal(1, countOfStudents);
        Assert.Equal(0, countOfStudentsAfterRemove);
    }

    [Fact]
    public void GetStreamGroupsByCourseNumber_StreamGroupsFound()
    {
        StreamGroup group10 = _isuExtraService.AddStreamGroup("M3210");
        StreamGroup group11 = _isuExtraService.AddStreamGroup("M3211");
        _isuExtraService.AddStreamGroup("M3111");

        IEnumerable<StreamGroup> streamGroups = _isuExtraService.GetStreamGroupsByCourseNumber(new CourseNumber(2));

        Assert.Contains(group10, streamGroups);
        Assert.Contains(group11, streamGroups);
    }

    [Fact]
    public void GetStreamStudentsWithoutOgnp_StreamStudentsWithoutOgnpFound()
    {
        Ognp ognp = _isuExtraService.AddOgnp("Методы ПО", 'V');
        StreamGroup group = _isuExtraService.AddStreamGroup("М3211");
        StreamStudent student1 = _isuExtraService.AddStudentToStreamGroup(group, "Student 1");
        OgnpFlow ognpFlow = _isuExtraService.AddOgnpFlowToOgnp("V 1.1", 'V', ognp);
        _isuExtraService.AddStudentToOgnpFlow(student1, ognpFlow);
        StreamStudent student2 = _isuExtraService.AddStudentToStreamGroup(group, "Student X");

        IEnumerable<StreamStudent> students = _isuExtraService.GetStreamStudentsWithoutOgnpFromStreamGroup(group);

        Assert.Contains(student2, students);
    }

    [Fact]
    public void AddStudentToOgnpFlow_StudentHasScheduleIntersection()
    {
        Ognp ognp = _isuExtraService.AddOgnp("Методы ПО", 'V');
        StreamGroup streamGroup = _isuExtraService.AddStreamGroup("M3211");
        StreamStudent student = _isuExtraService.AddStudentToStreamGroup(streamGroup, "Denis");
        OgnpFlow ognpFlow = _isuExtraService.AddOgnpFlowToOgnp("V 1.1", 'V', ognp);
        Schedule schedule = new Schedule().AddLesson(new Lesson("lesson", "teacher", 285, 2));
        streamGroup.AddSchedule(schedule);
        ognpFlow.AddSchedule(schedule);

        Assert.Throws<IsuExtraServiceException>(() => _isuExtraService.AddStudentToOgnpFlow(student, ognpFlow));
    }

    [Fact]
    public void AddStudentToOgnpFlow_StudentAddedToOgnpFlow()
    {
        Ognp ognp = _isuExtraService.AddOgnp("Кибербез", 'К');
        StreamGroup streamGroup = _isuExtraService.AddStreamGroup("M3211");
        var streamGroupSchedule = new Schedule();
        streamGroupSchedule.AddLesson(new Lesson("lesson", "teacher", 285, 2));
        streamGroup.AddSchedule(streamGroupSchedule);
        StreamStudent student = _isuExtraService.AddStudentToStreamGroup(streamGroup, "Denis");
        OgnpFlow ognpFlow = _isuExtraService.AddOgnpFlowToOgnp("КИБ 3.1", 'К', ognp);
        var ognpFlowSchedule = new Schedule();
        ognpFlowSchedule.AddLesson(new Lesson("lesson", "teacher", 285, 3));
        ognpFlow.AddSchedule(ognpFlowSchedule);

        _isuExtraService.AddStudentToOgnpFlow(student, ognpFlow);
        IEnumerable<StreamStudent> ognpFlowStudents = _isuExtraService.GetStreamStudentsFromOgnpFlow(ognpFlow);

        Assert.Contains(student, ognpFlowStudents);
    }
}