using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Models;

public class RestorePoint
{
    [JsonProperty]
    private readonly List<Storage> _storages;

    public RestorePoint(string name, int id)
    {
        if (name is null)
        {
            throw BackupsExceptionCollection.IsBlankOrNullException(nameof(name));
        }

        Id = id;
        Name = $"{name}_{id}";
        CreatedAt = DateTime.Now;
        _storages = new List<Storage>();
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }

    [JsonIgnore]
    public IReadOnlyCollection<Storage> Storages => _storages;

    public void AddStorage(Storage storage)
    {
        EnsureCanAddStorage(storage);

        _storages.Add(storage);
    }

    public bool IsContainsBackupObject(string path)
    {
        return _storages.Any(storage => storage.IsContainsBackupObject(path));
    }

    public void ChangeVersion(int id)
    {
        Id = id;
        Name = Path.GetFileNameWithoutExtension(Name)?.Split("_").Last() + $"_{id}";
        CreatedAt = DateTime.Now;
    }

    public void RenameFiles(int id)
    {
        foreach (Storage storage in _storages)
        {
            storage.ChangeVersion(id);
        }
    }

    private void EnsureCanAddStorage(Storage storage)
    {
        if (storage is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(storage));
        }

        if (_storages.Contains(storage))
        {
            throw BackupsExceptionCollection.NotUniqueException(nameof(storage));
        }
    }
}