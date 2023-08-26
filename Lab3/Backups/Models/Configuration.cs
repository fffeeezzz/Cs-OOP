using Backups.Repositories;
using Backups.SaveStrategies;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Models;

public class Configuration
{
    [JsonProperty]
    private readonly List<BackupObject> _backupObjects;

    [JsonProperty]
    private int _id = 1;

    public Configuration(string name, ISaveStrategy saveStrategy, AbstractRepository repository)
    {
        Name = name ?? throw BackupsExceptionCollection.IsBlankOrNullException(nameof(name));
        Repository = repository ?? throw BackupsExceptionCollection.IsNullException(nameof(repository));
        Job = new Job(saveStrategy);
        _backupObjects = new List<BackupObject>();
    }

    public string Name { get; }
    public AbstractRepository Repository { get; private set; }
    public Job Job { get; }

    [JsonIgnore]
    public IReadOnlyCollection<BackupObject> BackupObjects => _backupObjects;

    public void ChangeRepository(AbstractRepository repository)
    {
        Repository = repository ?? throw BackupsExceptionCollection.IsNullException(nameof(repository));
    }

    public void ChangeSaveStrategy(ISaveStrategy saveStrategy)
    {
        if (saveStrategy is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(saveStrategy));
        }

        Job.ChangeSaveStrategy(saveStrategy);
    }

    public void AddBackupObjects(params string[] paths)
    {
        if (paths is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(paths));
        }

        foreach (string path in paths)
        {
            _backupObjects.Add(new BackupObject(path));
        }
    }

    public void RemoveBackupObject(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw BackupsExceptionCollection.IsBlankOrNullException(nameof(path));
        }

        _backupObjects.Remove(new BackupObject(path));
    }

    public RestorePoint CreateBackup()
    {
        return Job.CreateBackup(_backupObjects, Repository, _id++);
    }
}