using Backups.Models;
using Backups.Repositories;
using Backups.Tools;

namespace Backups.SaveStrategies;

public class SingleSaveStrategy : ISaveStrategy
{
    public SaveStrategyType SaveStrategyType() => SaveStrategies.SaveStrategyType.SingleSaveStrategy;

    public IReadOnlyCollection<Storage> Save(
        List<BackupObject> backupObjects,
        AbstractRepository repository,
        int version)
    {
        EnsureCanSaveFiles(backupObjects, repository);

        string zipPath = Path.Combine(repository.RepositoryPath, $"Backup_{version}.zip");

        var storage = new Storage(zipPath);
        var filePaths = new List<string>();
        foreach (BackupObject backupObject in backupObjects.Where(backupObject =>
                     repository.IsFileExist(backupObject.Path) || repository.IsDirectoryExist(backupObject.Path)))
        {
            filePaths.Add(backupObject.Path);
            storage.AddBackupObject(backupObject);
        }

        repository.CreateZipArchive(zipPath, filePaths);

        return new List<Storage> { storage };
    }

    private void EnsureCanSaveFiles(List<BackupObject> backupObjects, AbstractRepository repository)
    {
        if (backupObjects is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backupObjects));
        }

        if (repository is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(repository));
        }
    }
}