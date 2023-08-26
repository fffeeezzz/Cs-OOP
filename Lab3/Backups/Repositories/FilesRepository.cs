using System.IO.Compression;
using Backups.Tools;

namespace Backups.Repositories;

public class FilesRepository : AbstractRepository
{
    public FilesRepository(string repositoryPath)
        : base(repositoryPath)
    {
    }

    public override RepositoryType Type() => RepositoryType.FilesRepository;

    public override bool IsFileExist(string path)
    {
        return File.Exists(path);
    }

    public override bool IsDirectoryExist(string path)
    {
        return Directory.Exists(path);
    }

    public override void CreateZipArchive(string path, List<string> filesPaths)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw BackupsExceptionCollection.IsBlankOrNullException(nameof(path));
        }

        if (filesPaths is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(filesPaths));
        }

        if (IsFileExist(path))
        {
            File.Delete(path);
        }

        using ZipArchive zipArchive = ZipFile.Open(path, ZipArchiveMode.Create);
        foreach (string filePath in filesPaths)
        {
            AddFileToZipArchive(zipArchive, filePath, string.Empty);
        }
    }

    public override byte[] ReadFile(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw BackupsExceptionCollection.IsBlankOrNullException(nameof(path));
        }

        if (!File.Exists(path))
        {
            throw BackupsExceptionCollection.FileDoesNotExistException(nameof(path));
        }

        return File.ReadAllBytes(path);
    }

    public override void RemoveFile(string path)
    {
        if (!File.Exists(path))
        {
            throw BackupsExceptionCollection.FileDoesNotExistException(nameof(path));
        }

        File.Delete(path);
    }

    public override void MoveFileToPath(string pathFrom, string pathTo, bool overwrite)
    {
        File.Move(pathFrom, pathTo, overwrite);
    }

    public override void ExtractFromZip(string pathFrom, string pathTo)
    {
        using ZipArchive zipArchive = ZipFile.OpenRead(pathFrom);
        zipArchive.ExtractToDirectory(pathTo);
    }

    protected void AddFileToZipArchive(ZipArchive zipArchive, string filePath, string dirName)
    {
        if (zipArchive is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(zipArchive));
        }

        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw BackupsExceptionCollection.IsBlankOrNullException(nameof(filePath));
        }

        if (dirName is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(dirName));
        }

        if (Path.HasExtension(filePath))
        {
            zipArchive.CreateEntryFromFile(filePath, Path.Combine(dirName, Path.GetFileName(filePath)));
        }
        else
        {
            AddDirectoryToZipArchive(zipArchive, filePath, Path.Combine(dirName, Path.GetFileName(filePath)));
        }
    }

    protected void AddDirectoryToZipArchive(ZipArchive zipArchive, string directoryPath, string dirName)
    {
        foreach (string fileSystemEntry in Directory.GetFileSystemEntries(directoryPath))
        {
            AddFileToZipArchive(zipArchive, fileSystemEntry, dirName);
        }
    }
}