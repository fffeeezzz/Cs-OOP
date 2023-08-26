using Backups.Extra.Logger;
using Backups.Extra.Models;
using Backups.Models;
using Backups.Services;
using Backups.Tools;

namespace Backups.Extra.Services;

public class BackupsExtraService
{
    public BackupsExtraService(
        ConfigurationExtra configurationExtra,
        ILogger logger,
        BackupsService backupsService = null)
    {
        BackupsService = backupsService ?? new BackupsService(configurationExtra);
        ConfigurationExtra = configurationExtra ??
                             throw BackupsExceptionCollection.IsNullException(nameof(configurationExtra));
        Logger = logger ?? throw BackupsExceptionCollection.IsNullException(nameof(logger));
    }

    public BackupsService BackupsService { get; }

    public ConfigurationExtra ConfigurationExtra { get; }

    public ILogger Logger { get; }

    public void AddFiles(params string[] paths)
    {
        BackupsService.AddFiles(paths);
    }

    public void DeleteFile(string path)
    {
        BackupsService.DeleteFile(path);
    }

    public RestorePoint GetLastRestorePoint()
    {
        return BackupsService.GetLastRestorePoint();
    }

    public Backup GetBackup()
    {
        return BackupsService.GetBackup();
    }

    public void ChangeConfiguration(ConfigurationExtra configuration)
    {
        BackupsService.ChangeConfiguration(configuration);
    }

    public void ExecuteBackupJob()
    {
        BackupsService.ExecuteBackupJob();

        Logger.Log($"Backup has been created");
    }

    public void Clean()
    {
        ConfigurationExtra.Clean(GetBackup());

        Logger.Log($"Backup has been cleaned");
    }

    public void Merge(int from)
    {
        ConfigurationExtra.Merge(GetBackup(), from);

        Logger.Log("Restore points has been merged");
    }

    public void Restore(int id, string pathToDir = null)
    {
        ConfigurationExtra.Restore(ConfigurationExtra.Repository, GetBackup(), id, pathToDir);

        Logger.Log($"Restore point with id - {id} has been restored");
    }
}