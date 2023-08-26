using Backups.Extra.Logger;
using Backups.Extra.Models;
using Backups.Extra.RestoreStrategies;
using Backups.Extra.SaveStrategiesExtra;
using Backups.Extra.SelectorStrategies;
using Backups.Extra.Services;
using Backups.Extra.Types;
using Backups.Models;
using Backups.Repositories;
using Backups.SaveStrategies;
using Xunit;

namespace Backups.Extra.Test;

public class BackupsExtraTests
{
    private readonly BackupsExtraService _backupsExtraService = new BackupsExtraService(
        new ConfigurationExtra(
            "fs",
            new SingleSaveStrategyExtra(new SingleSaveStrategy()),
            new SelectorStrategyByCount(2),
            new RestoreToOriginStrategy(),
            new LocalRepository("local")),
        new ConsoleLogger(LoggerType.IncludeDate));

    [Fact]
    public void CleanBackup_RestorePointHasBeenDeletedFromBackup()
    {
        string file1 = "/Users/denisportnov/RiderProjects/files/text1.rtf";
        string file2 = "/Users/denisportnov/RiderProjects/files/text2.rtf";
        string file3 = "/Users/denisportnov/RiderProjects/files/text3.rtf";
        _backupsExtraService.AddFiles(file1, file2);
        _backupsExtraService.ExecuteBackupJob();
        _backupsExtraService.ExecuteBackupJob();
        _backupsExtraService.AddFiles(file3);
        _backupsExtraService.ExecuteBackupJob();

        _backupsExtraService.Clean();
        IReadOnlyCollection<RestorePoint> restorePoints = _backupsExtraService.BackupsService.GetBackup().RestorePoints;

        Assert.Equal(2, restorePoints.Count);
        Assert.Equal(2, restorePoints.First().Storages.Single().BackupObjects.Count);
        Assert.Equal(2, restorePoints.First().Id);
        Assert.Equal(3, restorePoints.Last().Storages.Single().BackupObjects.Count);
        Assert.Equal(3, restorePoints.Last().Id);
    }

    [Fact]
    public void MergeRestorePoints_RestorePointHasBeenDeletedFromBackup()
    {
        string file1 = "/Users/denisportnov/RiderProjects/files/text1.rtf";
        string file2 = "/Users/denisportnov/RiderProjects/files/text2.rtf";
        string file3 = "/Users/denisportnov/RiderProjects/files/text3.rtf";
        _backupsExtraService.AddFiles(file1, file2);
        _backupsExtraService.ExecuteBackupJob();
        _backupsExtraService.ExecuteBackupJob();
        _backupsExtraService.AddFiles(file3);
        _backupsExtraService.ExecuteBackupJob();

        _backupsExtraService.Merge(2);
        IReadOnlyCollection<RestorePoint> restorePoints = _backupsExtraService.BackupsService.GetBackup().RestorePoints;

        Assert.Equal(2, restorePoints.Count);
        Assert.Equal(2, restorePoints.First().Storages.Single().BackupObjects.Count);
        Assert.Equal(1, restorePoints.First().Id);
        Assert.Equal(3, restorePoints.Last().Storages.Single().BackupObjects.Count);
        Assert.Equal(2, restorePoints.Last().Id);
    }
}