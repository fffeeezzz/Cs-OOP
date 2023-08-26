using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class Ognp : IEquatable<Ognp>
{
    private readonly List<OgnpFlow> _ognpFlows;

    public Ognp(string name, char faculty)
    {
        Validate(name, faculty);

        Name = name;
        Faculty = faculty;
        _ognpFlows = new List<OgnpFlow>();
    }

    public string Name { get; }
    public char Faculty { get; }
    public IEnumerable<OgnpFlow> OgnpFlows => _ognpFlows;

    public void AddOgnpFlow(OgnpFlow ognpFlow)
    {
        if (ognpFlow is null)
        {
            throw IsuExtraServiceExceptionCollection.IsNullException(nameof(ognpFlow));
        }

        if (_ognpFlows.Contains(ognpFlow))
        {
            throw IsuExtraServiceExceptionCollection.NotUniqueException(nameof(ognpFlow));
        }

        _ognpFlows.Add(ognpFlow);
    }

    public bool Equals(Ognp other)
    {
        return other != null &&
               Name == other.Name &&
               Faculty == other.Faculty;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Ognp);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Faculty);
    }

    private void Validate(string name, char faculty)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw IsuExtraServiceExceptionCollection.IsBlankOrNullException(name);
        }

        if (!char.IsLetter(faculty) || char.IsLower(faculty))
        {
            throw IsuExtraServiceExceptionCollection.InvalidFacultyLetter(nameof(faculty));
        }
    }
}