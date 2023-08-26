using Backups.Extra.Types;
using Backups.Models;

namespace Backups.Extra.SelectorStrategies;

public interface ISelectorStrategy
{
    List<RestorePoint> Execute(Backup backup);

    SelectorType Type();
}