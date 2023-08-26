using System.Text;
using Backups.Extra.Types;
using Backups.Tools;

namespace Backups.Extra.Logger;

public class FileLogger : ILogger
{
    public FileLogger(LoggerType type, string path)
    {
        Type = type;
        Path = path ?? throw BackupsExceptionCollection.IsBlankOrNullException(nameof(path));
    }

    public LoggerType Type { get; }

    public string Path { get; }

    public void Log(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw BackupsExceptionCollection.IsBlankOrNullException(nameof(message));
        }

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat($"{(Type is LoggerType.IncludeDate ? DateTime.Now : string.Empty)} ");
        stringBuilder.AppendFormat(message);

        new StreamWriter(Path, true).WriteLine(stringBuilder.ToString());
    }
}