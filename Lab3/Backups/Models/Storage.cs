using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Models;

public class Storage
{
    [JsonProperty]
    private readonly List<BackupObject> _backupObjects;

    public Storage(string storagePath)
    {
        StoragePath = storagePath ?? throw BackupsExceptionCollection.IsBlankOrNullException(nameof(storagePath));
        StorageName = Path.GetFileNameWithoutExtension(storagePath);
        _backupObjects = new List<BackupObject>();
    }

    public string StoragePath { get; private set; }
    public string StorageName { get; private set; }

    [JsonIgnore]
    public IReadOnlyCollection<BackupObject> BackupObjects => _backupObjects;

    public void AddBackupObject(BackupObject backupObject)
    {
        EnsureCanAddBackupObject(backupObject);

        _backupObjects.Add(backupObject);
    }

    public bool IsContainsBackupObject(string path)
    {
        var backupObject = new BackupObject(path);

        return _backupObjects.Contains(backupObject);
    }

    public void ChangeVersion(int id)
    {
        StoragePath = Path.GetDirectoryName(StoragePath) + $"_{id}.zip";
        StorageName = Path.GetFileNameWithoutExtension(StoragePath)?.Split("_").Last() + $"_{id}";
    }

    private void EnsureCanAddBackupObject(BackupObject backupObject)
    {
        if (backupObject is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backupObject));
        }

        if (_backupObjects.Contains(backupObject))
        {
            throw BackupsExceptionCollection.NotUniqueException(nameof(backupObject));
        }
    }
}