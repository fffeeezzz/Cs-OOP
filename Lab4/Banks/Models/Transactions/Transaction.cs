using Banks.Models.Bills;
using Banks.Tools;

namespace Banks.Models.Transactions;

public abstract class Transaction : IEquatable<Transaction>
{
    protected Transaction(decimal summary)
    {
        if (summary <= decimal.Zero)
        {
            throw BanksExceptionCollection.IsLowException(nameof(summary));
        }

        Id = Guid.NewGuid();
        Summary = summary;
    }

    public Guid Id { get; }
    public decimal Summary { get; }
    public Bank Bank { get; private set; }
    public Bill SenderBill { get; private set; }
    public Bill DestinationBill { get; private set; }

    public abstract TransactionType Type { get; }
    public abstract Bill Bill { get; }

    public void SetBank(Bank bank)
    {
        Bank = bank ?? throw BanksExceptionCollection.IsNullException(nameof(bank));
    }

    public void SetSenderBill(Bill bill)
    {
        SenderBill = bill ?? throw BanksExceptionCollection.IsNullException(nameof(bill));
    }

    public void SetDestinationBill(Bill bill)
    {
        DestinationBill = bill ?? throw BanksExceptionCollection.IsNullException(nameof(bill));
    }

    public abstract void Execute();
    public abstract void Cancel();

    public bool Equals(Transaction other)
    {
        return other != null && Equals(Id, other.Id);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Transaction);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}