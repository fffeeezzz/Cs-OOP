using Backups.Models;

namespace Backups.Services;

public interface IBackupsService
{
    void AddFiles(params string[] paths);

    void DeleteFile(string path);

    RestorePoint GetLastRestorePoint();

    Backup GetBackup();

    void ChangeConfiguration(Configuration configuration);

    void ExecuteBackupJob();
}