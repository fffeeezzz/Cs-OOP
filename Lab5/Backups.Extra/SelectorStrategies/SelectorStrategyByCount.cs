using Backups.Extra.Types;
using Backups.Models;
using Backups.Tools;

namespace Backups.Extra.SelectorStrategies;

public class SelectorStrategyByCount : ISelectorStrategy
{
    private const int MinAvailableValue = 0;

    public SelectorStrategyByCount(int countOfElements)
    {
        if (countOfElements < MinAvailableValue)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(countOfElements));
        }

        CountOfElements = countOfElements;
    }

    public int CountOfElements { get; }

    public SelectorType Type() => SelectorType.ByCount;

    public List<RestorePoint> Execute(Backup backup)
    {
        if (backup is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backup));
        }

        int countOfPoints = backup.RestorePoints.Count;
        return countOfPoints <= CountOfElements
            ? new List<RestorePoint>()
            : backup.RestorePoints.Take(countOfPoints - CountOfElements).ToList();
    }
}