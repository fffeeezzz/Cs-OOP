using Isu.Entities;
using Isu.Services;
using Isu.Tools;
using Xunit;

namespace Isu.Test;

public class IsuServiceTests
{
    private readonly IIsuService _sut = new IsuService();

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        Group group = _sut.AddGroup("M3211");

        Student student = _sut.AddStudent(group, "Denis Portnov");

        Assert.Equal(group, student.GetGroup());
        Assert.Contains(student, group.Students);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        Group group = _sut.AddGroup("M3211");
        for (int i = 0; i < group.MaxCountOfStudents; i++)
        {
            _sut.AddStudent(group, $"Student {i}");
        }

        Assert.Throws<IsuServiceException>(() => _sut.AddStudent(group, "Student X"));
    }

    [Theory]
    [InlineData("m321110")]
    [InlineData("m3211")]
    [InlineData("32110")]
    public void CreateGroupWithInvalidName_ThrowException(string groupName)
    {
        Assert.Throws<IsuServiceException>(() => _sut.AddGroup(groupName));
    }

    [Theory]
    [InlineData("M2211")]
    [InlineData("M3011")]
    [InlineData("M321a")]
    public void CreateGroupWithInvalidName_ThrowException2(string groupName)
    {
        Assert.Throws<IsuServiceException>(() => _sut.AddGroup(groupName));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        Group group = _sut.AddGroup("M3211");
        Group newGroup = _sut.AddGroup("M3311");
        Student student = _sut.AddStudent(group, "Denis Portnov");

        _sut.ChangeStudentGroup(student, newGroup);

        Assert.Equal(newGroup, student.GetGroup());
        Assert.Contains(student, newGroup.Students);
        Assert.DoesNotContain(student, group.Students);
    }
}