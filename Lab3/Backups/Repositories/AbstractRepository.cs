using System.IO.Compression;
using Backups.Tools;

namespace Backups.Repositories;

public abstract class AbstractRepository
{
    protected AbstractRepository(string repositoryPath)
    {
        RepositoryPath = repositoryPath ??
                         throw BackupsExceptionCollection.IsBlankOrNullException(nameof(repositoryPath));
        if (!Directory.Exists(repositoryPath))
        {
            Directory.CreateDirectory(repositoryPath);
        }
    }

    public string RepositoryPath { get; }

    public abstract RepositoryType Type();

    public abstract bool IsFileExist(string path);

    public abstract bool IsDirectoryExist(string path);

    public abstract void CreateZipArchive(string path, List<string> filesPaths);

    public abstract byte[] ReadFile(string path);

    public abstract void RemoveFile(string path);

    public abstract void MoveFileToPath(string pathFrom, string pathTo, bool overwrite);

    public abstract void ExtractFromZip(string pathFrom, string pathTo);
}