using Backups.Models;
using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Services;

public class BackupsService : IBackupsService
{
    public BackupsService(Configuration configuration, Backup backup = null)
    {
        Configuration = configuration ?? throw BackupsExceptionCollection.IsNullException(nameof(configuration));
        Backup = backup ?? new Backup();
    }

    public Backup Backup { get; }
    public Configuration Configuration { get; internal set; }

    public void AddFiles(params string[] paths)
    {
        Configuration.AddBackupObjects(paths);
    }

    public void DeleteFile(string path)
    {
        Configuration.RemoveBackupObject(path);
    }

    public RestorePoint GetLastRestorePoint()
    {
        return Backup.RestorePoints.LastOrDefault();
    }

    public Backup GetBackup()
    {
        return Backup;
    }

    public void ChangeConfiguration(Configuration configuration)
    {
        if (configuration is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(configuration));
        }

        Configuration = configuration;
    }

    public void ExecuteBackupJob()
    {
        Backup.AddRestorePoint(Configuration.CreateBackup());
    }
}