using Backups.Models;
using Backups.Repositories;
using Backups.SaveStrategies;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Extra.SaveStrategiesExtra;

public class SingleSaveStrategyExtra : ISaveStrategyExtra
{
    [JsonProperty]
    private readonly ISaveStrategy _strategy;

    public SingleSaveStrategyExtra(ISaveStrategy strategy)
    {
        _strategy = strategy ?? throw BackupsExceptionCollection.IsNullException(nameof(strategy));
    }

    public SaveStrategyType SaveStrategyType() => SaveStrategies.SaveStrategyType.SingleSaveStrategy;

    public IReadOnlyCollection<Storage> Save(
        List<BackupObject> backupObjects,
        AbstractRepository repository,
        int version) => _strategy.Save(backupObjects, repository, version);

    public void Remove(
        List<RestorePoint> restorePoints,
        Backup backup,
        AbstractRepository repository)
    {
        EnsureArgumentsToRemoveAreValid(restorePoints, backup, repository);

        foreach (RestorePoint restorePoint in restorePoints)
        {
            backup.RemoveRestorePoint(restorePoint);
            repository.RemoveFile(restorePoint.Storages.Single().StoragePath);
        }
    }

    public void Merge(Backup backup, int from, AbstractRepository repository)
    {
        EnsureArgumentsToMergeAreValid(backup, repository);

        var restorePoints = backup.RestorePoints.Skip(from - 1).ToList();
        RestorePoint lastPoint = restorePoints.Last();

        var newPoint = new RestorePoint("Backup", from);
        var backupObjects = new List<BackupObject>();
        foreach (Storage lastPointStorage in lastPoint.Storages)
        {
            newPoint.AddStorage(lastPointStorage);
            backupObjects.AddRange(lastPointStorage.BackupObjects);
        }

        foreach (RestorePoint restorePoint in restorePoints)
        {
            backup.RemoveRestorePoint(restorePoint);
            repository.RemoveFile(restorePoint.Storages.Single().StoragePath);
        }

        backup.AddRestorePoint(newPoint);
        Save(backupObjects, repository, from);
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