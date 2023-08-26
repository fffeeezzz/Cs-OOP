using Backups.Models;
using Backups.Repositories;

namespace Backups.SaveStrategies;

public interface ISaveStrategy
{
    SaveStrategyType SaveStrategyType();

    IReadOnlyCollection<Storage> Save(List<BackupObject> backupObjects, AbstractRepository repository, int version);
}