using Backups.Extra.Logger;
using Backups.Extra.Models;
using Backups.Extra.RestoreStrategies;
using Backups.Extra.SaveStrategiesExtra;
using Backups.Extra.SelectorStrategies;
using Backups.Extra.Services;
using Backups.Extra.Types;
using Backups.Repositories;
using Backups.SaveStrategies;

namespace Backups.Extra;

internal static class Program
{
    private static void Main()
    {
        var config = new ConfigurationExtra(
            "fs",
            new SingleSaveStrategyExtra(new SingleSaveStrategy()),
            new SelectorStrategyByCount(2),
            new RestoreToOriginStrategy(),
            new FilesRepository("/Users/denisportnov/RiderProjects/SingleStrategy"));

        var backupsService = new BackupsExtraService(config, new ConsoleLogger(LoggerType.IncludeDate));

        backupsService.AddFiles(
            "/Users/denisportnov/RiderProjects/files/text.rtf",
            "/Users/denisportnov/RiderProjects/files/text2.rtf");

        backupsService.ExecuteBackupJob();
        backupsService.ExecuteBackupJob();

        backupsService.AddFiles("/Users/denisportnov/RiderProjects/files/os lab5.docx");
        backupsService.ExecuteBackupJob();

        backupsService.Clean();

        ResurgenceSystem.Serialize(
            backupsService,
            "/Users/denisportnov/RiderProjects/OOP/Lab5/Backups.Extra/System.json");
    }
}