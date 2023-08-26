using Backups.Extra.Types;
using Backups.Models;
using Backups.Repositories;

namespace Backups.Extra.RestoreStrategies;

public interface IRestoreStrategy
{
    void Execute(AbstractRepository repository, Backup backup, int id, string pathToDir = null);

    RestoreType Type();
}