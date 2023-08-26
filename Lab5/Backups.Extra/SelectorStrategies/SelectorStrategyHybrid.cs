using Backups.Extra.Types;
using Backups.Models;
using Backups.Tools;

namespace Backups.Extra.SelectorStrategies;

public class SelectorStrategyHybrid : ISelectorStrategy
{
    public SelectorStrategyHybrid(ISelectorStrategy[] strategies, SelectorRequirements requirements)
    {
        Strategies = strategies ?? throw BackupsExceptionCollection.IsNullException(nameof(strategies));
        Requirements = requirements;
    }

    public ISelectorStrategy[] Strategies { get; }

    public SelectorRequirements Requirements { get; }

    public SelectorType Type() => SelectorType.Hybrid;

    public List<RestorePoint> Execute(Backup backup)
    {
        if (backup is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(backup));
        }

        return Requirements switch
        {
            SelectorRequirements.Union => UnionSelect(backup),
            SelectorRequirements.Or => OrSelect(backup),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private List<RestorePoint> UnionSelect(Backup backup)
    {
        var restorePoints = new List<RestorePoint>();
        ISelectorStrategy timeSelector = Strategies.FirstOrDefault(s => s.Type() == SelectorType.ByDate);
        if (timeSelector is not null)
        {
            restorePoints.AddRange(timeSelector.Execute(backup));
        }

        return Strategies.Aggregate(restorePoints, (points, selector)
            => points.Intersect(selector.Execute(backup)).ToList());
    }

    private List<RestorePoint> OrSelect(Backup backup)
    {
        var restorePoints = new List<RestorePoint>();
        return Strategies.Aggregate(restorePoints, (current, selector)
            => current.Union(selector.Execute(backup)).ToList());
    }
}