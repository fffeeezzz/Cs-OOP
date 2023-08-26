using Banks.Tools;

namespace Banks.Models.Bills;

public class DepositPercent : IEquatable<DepositPercent>, IComparable<DepositPercent>
{
    private const decimal MaxPercent = 100;

    public DepositPercent(decimal summary, decimal percent)
    {
        if (percent > MaxPercent)
        {
            throw BanksExceptionCollection.IsHighException(nameof(percent));
        }

        if (summary < decimal.Zero)
        {
            throw BanksExceptionCollection.IsLowException(nameof(summary));
        }

        Percent = percent;
        Summary = summary;
    }

    public decimal Percent { get; private set; }
    public decimal Summary { get; private set; }

    public void ChangePercent(decimal percent)
    {
        if (percent > MaxPercent)
        {
            throw BanksExceptionCollection.IsHighException(nameof(percent));
        }

        Percent = percent;
    }

    public void ChangeSummary(decimal summary)
    {
        if (summary < decimal.Zero)
        {
            throw BanksExceptionCollection.IsLowException(nameof(summary));
        }

        Summary = summary;
    }

    public bool Equals(DepositPercent other)
    {
        return other != null && Summary == other.Summary;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as DepositPercent);
    }

    public override int GetHashCode() => Summary.GetHashCode();

    public int CompareTo(DepositPercent other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        return ReferenceEquals(null, other) ? 1 : Summary.CompareTo(other.Summary);
    }
}