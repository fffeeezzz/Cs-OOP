using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Extra.Services;

public static class ResurgenceSystem
{
    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented,
    };

    public static void Serialize(BackupsExtraService service, string path)
    {
        if (service is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(service));
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            throw BackupsExceptionCollection.IsBlankOrNullException(nameof(path));
        }

        File.AppendAllText(path, JsonConvert.SerializeObject(service, Settings));
    }

    public static BackupsExtraService Deserialize(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw BackupsExceptionCollection.IsBlankOrNullException(nameof(path));
        }

        return JsonConvert.DeserializeObject<BackupsExtraService>(File.ReadAllText(path), Settings);
    }
}