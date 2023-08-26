using Backups.Extra.Types;
using Backups.Models;
using Backups.Tools;

namespace Backups.Extra.SelectorStrategies;

public class SelectorStrategyByDate : ISelectorStrategy
{
    public SelectorStrategyByDate(DateTime dateTime)
    {
        DateLimit = dateTime;
    }

    public DateTime DateLimit { get; }

    public SelectorType Type() => SelectorType.ByDate;

    public List<RestorePoint> Execute(Backup backup)
    {
        if (backup is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backup));
        }

        return backup.RestorePoints.Where(point => point.CreatedAt < DateLimit).ToList();
    }
}