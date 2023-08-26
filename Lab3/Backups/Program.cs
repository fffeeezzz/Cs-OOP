using Backups.Models;
using Backups.Repositories;
using Backups.SaveStrategies;
using Backups.Services;

namespace Backups;

internal static class Program
{
    private static void Main()
    {
        var config = new Configuration(
            "fs",
            new SingleSaveStrategy(),
            new FilesRepository("/Users/denisportnov/RiderProjects/SingleStrategy"));

        IBackupsService backupsService = new BackupsService(config);

        backupsService.AddFiles(
            "/Users/denisportnov/RiderProjects/files/text.rtf",
            "/Users/denisportnov/RiderProjects/files/text2.rtf",
            "/Users/denisportnov/Desktop/tmp");

        backupsService.ExecuteBackupJob();
        backupsService.ExecuteBackupJob();
    }
}