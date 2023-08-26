using Backups.Models;
using Backups.Repositories;
using Backups.SaveStrategies;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Extra.SaveStrategiesExtra;

public class SplitSaveStrategyExtra : ISaveStrategyExtra
{
    [JsonProperty]
    private readonly ISaveStrategy _strategy;

    public SplitSaveStrategyExtra(ISaveStrategy strategy)
    {
        _strategy = strategy ?? throw BackupsExceptionCollection.IsNullException(nameof(strategy));
    }

    public SaveStrategyType SaveStrategyType() => SaveStrategies.SaveStrategyType.SplitSaveStrategy;

    public IReadOnlyCollection<Storage> Save(
        List<BackupObject> backupObjects,
        AbstractRepository repository,
        int version) => _strategy.Save(backupObjects, repository, version);

    public void Remove(List<RestorePoint> restorePoints, Backup backup, AbstractRepository repository)
    {
        EnsureArgumentsToRemoveAreValid(restorePoints, backup, repository);

        foreach (RestorePoint restorePoint in restorePoints)
        {
            backup.RemoveRestorePoint(restorePoint);
            foreach (Storage storage in restorePoint.Storages)
            {
                repository.RemoveFile(storage.StoragePath);
            }
        }
    }

    public void Merge(Backup backup, int from, AbstractRepository repository)
    {
        EnsureArgumentsToMergeAreValid(backup, repository);

        var restorePoints = backup.RestorePoints.Skip(from).Reverse().ToList();
        RestorePoint lastPoint = restorePoints.First();

        foreach (RestorePoint restorePoint in restorePoints.Where(point => point.Id != lastPoint.Id))
        {
            foreach (Storage storage in restorePoint.Storages)
            {
                if (!lastPoint.IsContainsBackupObject(storage.BackupObjects.Single().Path))
                {
                    lastPoint.AddStorage(storage);
                }

                repository.RemoveFile(storage.StoragePath);
            }

            backup.RemoveRestorePoint(restorePoint);
        }

        Remove(restorePoints, backup, repository);

        var backupObjects = new List<BackupObject>();
        foreach (Storage storage in lastPoint.Storages)
        {
            backupObjects.AddRange(storage.BackupObjects);
        }

        IReadOnlyCollection<Storage> storages = Save(backupObjects, repository, from);
        var newPoint = new RestorePoint(repository.RepositoryPath, from);
        foreach (Storage storage in storages)
        {
            newPoint.AddStorage(storage);
        }

        backup.AddRestorePoint(newPoint);
    }

    private void EnsureArgumentsToRemoveAreValid(
        List<RestorePoint> restorePoints,
        Backup backup,
        AbstractRepository repository)
    {
        if (restorePoints is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(restorePoints));
        }

        if (backup is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backup));
        }

        if (repository is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(repository));
        }
    }

    private void EnsureArgumentsToMergeAreValid(Backup backup, AbstractRepository repository)
    {
        if (backup is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backup));
        }

        if (repository is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(repository));
        }
    }
}