using Backups.Extra.Types;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;

namespace Backups.Extra.RestoreStrategies;

public class RestoreToOriginStrategy : IRestoreStrategy
{
    public RestoreType Type() => RestoreType.Origin;

    public void Execute(AbstractRepository repository, Backup backup, int id, string pathToDir = null)
    {
        EnsureArgumentsAreValid(repository, backup);

        RestorePoint restorePoint = backup.RestorePoints.FirstOrDefault(r => r.Id == id);
        if (restorePoint is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(restorePoint));
        }

        foreach (Storage storage in restorePoint.Storages)
        {
            repository.ExtractFromZip(storage.StoragePath, repository.RepositoryPath);
            foreach (BackupObject backupObject in storage.BackupObjects)
            {
                repository.MoveFileToPath(
                    Path.Combine(repository.RepositoryPath, backupObject.Name),
                    backupObject.Path,
                    true);
            }
        }
    }

    private void EnsureArgumentsAreValid(AbstractRepository repository, Backup backup)
    {
        if (repository is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(repository));
        }

        if (backup is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backup));
        }
    }
}