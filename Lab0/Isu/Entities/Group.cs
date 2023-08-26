using Isu.Models;
using Isu.Tools;

namespace Isu.Entities;

public class Group
{
    private const int MaxCountStudents = 25;
    private readonly List<Student> _students = new List<Student>();

    public Group(GroupName groupName)
    {
        GroupName = groupName ?? throw IsuServiceValidationExceptionCollection.IsNullException(nameof(groupName));
    }

    public int MaxCountOfStudents => MaxCountStudents;
    public GroupName GroupName { get; }
    public IReadOnlyList<Student> Students => _students;

    public Student AddStudent(Student student)
    {
        EnsureCanAddStudent(student);

        _students.Add(student);

        return student;
    }

    public void RemoveStudent(Student student)
    {
        if (student is null)
        {
            throw IsuServiceValidationExceptionCollection.IsNullException(nameof(student));
        }

        _students.Remove(student);
    }

    private void EnsureCanAddStudent(Student student)
    {
        if (_students.Count >= MaxCountStudents)
        {
            throw IsuServiceValidationExceptionCollection.ToManyElementsException(
                MaxCountStudents,
                nameof(_students));
        }

        if (student is null)
        {
            throw IsuServiceValidationExceptionCollection.IsNullException(nameof(student));
        }

        if (_students.FirstOrDefault(x => x.IsuNumber == student.IsuNumber) is not null)
        {
            throw IsuServiceValidationExceptionCollection.NotUniqueStudentException(student, nameof(student));
        }
    }
}