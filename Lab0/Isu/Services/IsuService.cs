using Isu.Entities;
using Isu.Models;
using Isu.Tools;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private readonly Dictionary<Group, List<Student>> _studentsByGroup;
    private int _nextIsuNumber = 100000;

    public IsuService()
    {
        _studentsByGroup = new Dictionary<Group, List<Student>>();
    }

    public Group AddGroup(string groupName)
    {
        var group = new Group(new GroupName(groupName));

        if (IsGroupExist(group))
        {
            throw IsuServiceValidationExceptionCollection.NotUniqueGroupException(group, nameof(group));
        }

        _studentsByGroup.Add(group, new List<Student>());

        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        if (!IsGroupExist(group))
        {
            throw IsuServiceValidationExceptionCollection.NotExistingGroupException(group, nameof(group));
        }

        var student = new Student(_nextIsuNumber++, name)
        {
            Group = group,
        };

        group.AddStudent(student);
        _studentsByGroup[group].Add(student);

        return student;
    }

    public Student GetStudent(int isuNumber)
    {
        return _studentsByGroup.Values.SelectMany(x => x).FirstOrDefault(student => student.IsuNumber == isuNumber) ??
               throw IsuServiceValidationExceptionCollection.NotExistingStudentByIsuNumberException(
                   isuNumber,
                   nameof(isuNumber));
    }

    public Student FindStudent(int id)
    {
        try
        {
            return GetStudent(id);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public IEnumerable<Student> FindStudents(GroupName groupName)
    {
        return _studentsByGroup.Keys.Where(x => x.GroupName.Name == groupName.Name).SelectMany(x => x.Students);
    }

    public IEnumerable<Student> FindStudents(CourseNumber courseNumber)
    {
        return FindGroups(courseNumber).SelectMany(x => x.Students);
    }

    public Group FindGroup(GroupName groupName)
    {
        return _studentsByGroup.Keys.FirstOrDefault(group => group.GroupName == groupName);
    }

    public IEnumerable<Group> FindGroups(CourseNumber courseNumber)
    {
        return _studentsByGroup.Keys.Where(x => x.GroupName.CourseNumber.Number == courseNumber.Number);
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        EnsureGroupAndStudentValid(student, newGroup);
        Group oldGroup = student.Group;

        student.Group.RemoveStudent(student);
        student.Group = newGroup;
        newGroup.AddStudent(student);

        _studentsByGroup[oldGroup].Remove(student);
        _studentsByGroup[newGroup].Add(student);
    }

    private bool IsGroupExist(Group group)
    {
        return _studentsByGroup.ContainsKey(group);
    }

    private void EnsureGroupAndStudentValid(Student student, Group group)
    {
        if (student is null)
        {
            throw IsuServiceValidationExceptionCollection.IsNullException(nameof(student));
        }

        if (group is null)
        {
            throw IsuServiceValidationExceptionCollection.IsNullException(nameof(group));
        }

        if (FindStudent(student.IsuNumber) is null)
        {
            throw IsuServiceValidationExceptionCollection.NotExistingStudentException(student, nameof(student));
        }

        if (FindGroup(group.GroupName) is null)
        {
            throw IsuServiceValidationExceptionCollection.NotExistingGroupException(group, nameof(group));
        }
    }
}