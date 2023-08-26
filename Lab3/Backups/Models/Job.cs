using Backups.Repositories;
using Backups.SaveStrategies;
using Backups.Tools;

namespace Backups.Models;

public class Job
{
    public Job(ISaveStrategy saveStrategy)
    {
        EnsureSaveStrategyIsNotNull(saveStrategy);

        SaveStrategy = saveStrategy;
    }

    public ISaveStrategy SaveStrategy { get; private set; }

    public RestorePoint CreateBackup(
        List<BackupObject> backupObjects,
        AbstractRepository repository,
        int version)
    {
        if (backupObjects is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backupObjects));
        }

        if (repository is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(repository));
        }

        IReadOnlyCollection<Storage> storages = SaveStrategy.Save(backupObjects, repository, version);
        var restorePoint = new RestorePoint(repository.RepositoryPath, version);
        foreach (Storage storage in storages)
        {
            restorePoint.AddStorage(storage);
        }

        return restorePoint;
    }

    public void ChangeSaveStrategy(ISaveStrategy saveStrategy)
    {
        EnsureSaveStrategyIsNotNull(saveStrategy);

        SaveStrategy = saveStrategy;
    }

    private void EnsureSaveStrategyIsNotNull(ISaveStrategy saveStrategy)
    {
        if (saveStrategy is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(saveStrategy));
        }
    }
}