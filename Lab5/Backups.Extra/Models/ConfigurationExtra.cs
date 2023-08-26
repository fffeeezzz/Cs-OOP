using Backups.Extra.RestoreStrategies;
using Backups.Extra.SaveStrategiesExtra;
using Backups.Extra.SelectorStrategies;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;

namespace Backups.Extra.Models;

public class ConfigurationExtra : Configuration
{
    public ConfigurationExtra(
        string name,
        ISaveStrategyExtra saveStrategyExtra,
        ISelectorStrategy selectorStrategy,
        IRestoreStrategy restoreStrategy,
        AbstractRepository repository)
        : base(name, saveStrategyExtra, repository)
    {
        Validate(name, saveStrategyExtra, selectorStrategy, restoreStrategy, repository);

        Job = new JobExtra();
        SaveStrategyExtra = saveStrategyExtra;
        SelectorStrategy = selectorStrategy;
        RestoreStrategy = restoreStrategy;
    }

    public ISaveStrategyExtra SaveStrategyExtra { get; }

    public ISelectorStrategy SelectorStrategy { get; }

    public IRestoreStrategy RestoreStrategy { get; }

    public new JobExtra Job { get; }

    public void Clean(Backup backup)
    {
        Job.Clean(backup, Repository, SelectorStrategy, SaveStrategyExtra);
    }

    public void Merge(Backup backup, int from)
    {
        Job.Merge(backup, from, Repository, SaveStrategyExtra);
    }

    public void Restore(AbstractRepository repository, Backup backup, int id, string pathToDir = null)
    {
        Job.Restore(repository, backup, RestoreStrategy, id, pathToDir);
    }

    private void Validate(
        string name,
        ISaveStrategyExtra saveStrategyExtra,
        ISelectorStrategy selectorStrategy,
        IRestoreStrategy restoreStrategy,
        AbstractRepository repository)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw BackupsExceptionCollection.IsBlankOrNullException(nameof(name));
        }

        if (saveStrategyExtra is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(saveStrategyExtra));
        }

        if (selectorStrategy is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(selectorStrategy));
        }

        if (restoreStrategy is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(restoreStrategy));
        }

        if (repository is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(repository));
        }
    }
}