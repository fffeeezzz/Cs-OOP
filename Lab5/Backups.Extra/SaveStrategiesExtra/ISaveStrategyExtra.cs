using Backups.Models;
using Backups.Repositories;
using Backups.SaveStrategies;

namespace Backups.Extra.SaveStrategiesExtra;

public interface ISaveStrategyExtra : ISaveStrategy
{
    void Remove(List<RestorePoint> restorePoints, Backup backup, AbstractRepository repository);

    void Merge(Backup backup, int from, AbstractRepository repository);
}