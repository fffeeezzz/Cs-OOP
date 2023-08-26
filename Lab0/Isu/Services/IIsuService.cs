using System.Collections;
using Isu.Entities;
using Isu.Models;

namespace Isu.Services;

public interface IIsuService
{
    Group AddGroup(string groupName);
    Student AddStudent(Group group, string name);

    Student GetStudent(int isuNumber);
    Student FindStudent(int id);
    IEnumerable<Student> FindStudents(GroupName groupName);
    IEnumerable<Student> FindStudents(CourseNumber courseNumber);

    Group FindGroup(GroupName groupName);
    IEnumerable<Group> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group newGroup);
}