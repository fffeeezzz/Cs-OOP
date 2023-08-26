using Backups.Tools;

namespace Backups.Models;

public class BackupObject : IEquatable<BackupObject>
{
    public BackupObject(string path)
    {
        Path = path ?? throw BackupsExceptionCollection.IsBlankOrNullException(nameof(path));
        Name = System.IO.Path.GetFileName(path);
    }

    public string Path { get; }
    public string Name { get; }

    public bool Equals(BackupObject other)
    {
        return other != null && Path == other.Path;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as BackupObject);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Path);
    }
}