using System.IO.Compression;

namespace Backups.Repositories;

public class LocalRepository : AbstractRepository
{
    public LocalRepository(string repositoryPath)
        : base(repositoryPath)
    {
    }

    public override RepositoryType Type() => RepositoryType.LocalRepository;

    public override bool IsFileExist(string path)
    {
        return true;
    }

    public override bool IsDirectoryExist(string path)
    {
        return true;
    }

    public override void CreateZipArchive(string path, List<string> filesPaths)
    {
    }

    public override byte[] ReadFile(string path)
    {
        using var memoryStream = new MemoryStream();
        using var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create);
        zip.CreateEntry("text.txt");

        return memoryStream.ToArray();
    }

    public override void RemoveFile(string path) { }

    public override void MoveFileToPath(string pathFrom, string pathTo, bool overwrite) { }

    public override void ExtractFromZip(string pathFrom, string pathTo) { }
}