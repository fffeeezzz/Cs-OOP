using Backups.Tools;
using Newtonsoft.Json;

namespace Backups.Models;

public class Backup
{
    [JsonProperty]
    private readonly List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    [JsonIgnore]
    public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints;

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        if (restorePoint is null)
        {
            throw BackupsExceptionCollection.IsNullException(nameof(restorePoint));
        }

        _restorePoints.Add(restorePoint);
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        _restorePoints.Remove(restorePoint);
    }
}