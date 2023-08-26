using System.Text;
using Backups.Extra.Types;
using Backups.Tools;

namespace Backups.Extra.Logger;

public class ConsoleLogger : Backups.Extra.Logger.ILogger
{
    public ConsoleLogger(LoggerType type)
    {
        Type = type;
    }

    public LoggerType Type { get; }

    public void Log(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw BackupsExceptionCollection.IsBlankOrNullException(nameof(message));
        }

        var stringBuilder = new StringBuilder();

        stringBuilder.AppendFormat($"{(Type is LoggerType.IncludeDate ? DateTime.Now : string.Empty)} ");
        stringBuilder.AppendFormat(message);

        Console.WriteLine(stringBuilder.ToString());
    }
}