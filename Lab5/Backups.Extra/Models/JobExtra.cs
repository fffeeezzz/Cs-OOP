using Backups.Extra.RestoreStrategies;
using Backups.Extra.SaveStrategiesExtra;
using Backups.Extra.SelectorStrategies;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;

namespace Backups.Extra.Models;

public class JobExtra
{
    public RestorePoint CreateBackup(
        List<BackupObject> backupObjects,
        AbstractRepository repository,
        ISaveStrategyExtra saveStrategyExtra,
        int version)
    {
        Validate(backupObjects, repository, saveStrategyExtra);

        IReadOnlyCollection<Storage> storages = saveStrategyExtra.Save(backupObjects, repository, version);
        var restorePoint = new RestorePoint(repository.RepositoryPath, version);
        foreach (Storage storage in storages)
        {
            restorePoint.AddStorage(storage);
        }

        return restorePoint;
    }

    public void Restore(
        AbstractRepository repository,
        Backup backup,
        IRestoreStrategy restoreStrategy,
        int id,
        string pathToDir = null)
    {
        restoreStrategy.Execute(repository, backup, id, pathToDir);
    }

    public void Clean(
        Backup backup,
        AbstractRepository repository,
        ISelectorStrategy selectorStrategy,
        ISaveStrategyExtra saveStrategyExtra)
    {
        EnsureArgumentsAreValid(backup, repository, selectorStrategy, saveStrategyExtra);

        List<RestorePoint> restorePoints = selectorStrategy.Execute(backup);
        saveStrategyExtra.Remove(restorePoints, backup, repository);
    }

    public void Merge(Backup backup, int from, AbstractRepository repository, ISaveStrategyExtra saveStrategyExtra)
    {
        saveStrategyExtra.Merge(backup, from, repository);
    }

    private void Validate(
        List<BackupObject> backupObjects,
        AbstractRepository repository,
        ISaveStrategyExtra saveStrategyExtra)
    {
        if (backupObjects is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backupObjects));
        }

        if (repository is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(repository));
        }

        if (saveStrategyExtra is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(saveStrategyExtra));
        }
    }

    private void EnsureArgumentsAreValid(
        Backup backup,
        AbstractRepository repository,
        ISelectorStrategy selectorStrategy,
        ISaveStrategyExtra saveStrategyExtra)
    {
        if (backup is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backup));
        }

        if (repository is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(repository));
        }

        if (selectorStrategy is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(selectorStrategy));
        }

        if (saveStrategyExtra is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(selectorStrategy));
        }
    }
}