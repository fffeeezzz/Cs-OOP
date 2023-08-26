using Banks.Tools;

namespace Banks.Time;

public class TimeProvider
{
    private const int Month = 30;

    public TimeProvider()
    {
        CurrentTime = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public DateTime CurrentTime { get; private set; }
    public DateTime UpdatedAt { get; internal set; }

    public bool IsTimeToUpdate()
    {
        return CurrentTime >= UpdatedAt.AddDays(Month);
    }

    public void SkipDays(int dayCount)
    {
        if (dayCount < 0)
        {
            throw BanksExceptionCollection.IsNullException(nameof(dayCount));
        }

        CurrentTime = CurrentTime.AddDays(dayCount);
    }
}