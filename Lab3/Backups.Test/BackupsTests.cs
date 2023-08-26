using Backups.Models;
using Backups.Repositories;
using Backups.SaveStrategies;
using Backups.Services;
using Xunit;

namespace Backups.Test;

public class BackupsTests
{
    private readonly IBackupsService _backupsService =
        new BackupsService(new Configuration("fs", new SplitSaveStrategy(), new LocalRepository("local")));

    [Fact]
    public void BackupFiles_RestorePointHasTheseFiles()
    {
        string file1 = "/Users/denisportnov/RiderProjects/files/text1.rtf";
        string file2 = "/Users/denisportnov/RiderProjects/files/text2.rtf";
        _backupsService.AddFiles(file1, file2);

        _backupsService.ExecuteBackupJob();
        RestorePoint restorePoint = _backupsService.GetLastRestorePoint();

        Assert.Equal(2, restorePoint.Storages.Count);
        Assert.True(restorePoint.IsContainsBackupObject(file1));
        Assert.True(restorePoint.IsContainsBackupObject(file2));
    }

    [Fact]
    public void BackupFilesAfterRemoveBackupObject_RestorePointHasOnlyAddedFiles()
    {
        string file1 = "/Users/denisportnov/RiderProjects/files/text1.rtf";
        string file2 = "/Users/denisportnov/RiderProjects/files/text2.rtf";
        string file3 = "/Users/denisportnov/RiderProjects/files/text3.rtf";
        _backupsService.AddFiles(file1, file2, file3);
        _backupsService.ExecuteBackupJob();
        _backupsService.ExecuteBackupJob();

        _backupsService.DeleteFile(file2);
        _backupsService.ExecuteBackupJob();
        RestorePoint restorePoint = _backupsService.GetLastRestorePoint();

        Assert.Equal(2, restorePoint.Storages.Count);
        Assert.True(restorePoint.IsContainsBackupObject(file1));
        Assert.True(restorePoint.IsContainsBackupObject(file3));
        Assert.False(restorePoint.IsContainsBackupObject(file2));
    }
}