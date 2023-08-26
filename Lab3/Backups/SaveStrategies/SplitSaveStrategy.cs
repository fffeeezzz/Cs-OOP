using Backups.Models;
using Backups.Repositories;
using Backups.Tools;

namespace Backups.SaveStrategies;

public class SplitSaveStrategy : ISaveStrategy
{
    public SaveStrategyType SaveStrategyType() => SaveStrategies.SaveStrategyType.SplitSaveStrategy;

    public IReadOnlyCollection<Storage> Save(
        List<BackupObject> backupObjects,
        AbstractRepository repository,
        int version)
    {
        EnsureCanSaveFiles(backupObjects, repository);

        var storages = new List<Storage>();
        foreach (BackupObject backupObject in backupObjects.Where(backupObject =>
                     repository.IsFileExist(backupObject.Path) || repository.IsDirectoryExist(backupObject.Path)))
        {
            string zipPath = Path.Combine(
                repository.RepositoryPath,
                $"{Path.GetFileNameWithoutExtension(backupObject.Path)}_{version}.zip");

            repository.CreateZipArchive(zipPath, new List<string> { backupObject.Path });

            var storage = new Storage(zipPath);
            storage.AddBackupObject(backupObject);
            storages.Add(storage);
        }

        return storages;
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