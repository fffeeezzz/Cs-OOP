using Banks.Tools;

namespace Banks.Models.Bills;

public abstract class Bill
{
    protected Bill()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now.Ticks;
        Balance = decimal.Zero;
        Savings = decimal.Zero;
    }

    public Guid Id { get; }
    public long CreatedAt { get; }
    public decimal Balance { get; internal set; }
    public decimal Savings { get; private set; }

    public void ChangeSavings(decimal savings)
    {
        if (savings < decimal.Zero)
        {
            throw BanksExceptionCollection.IsLowException(nameof(savings));
        }

        Savings = savings;
    }

    public abstract BillType BillType();
    internal abstract void PaySaving();
}