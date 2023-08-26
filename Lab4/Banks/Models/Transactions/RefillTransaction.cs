using Banks.Models.Bills;

namespace Banks.Models.Transactions;

public class RefillTransaction : Transaction
{
    public RefillTransaction(decimal summary, Bill bill)
        : base(summary)
    {
        SetSenderBill(bill);
    }

    public override TransactionType Type => TransactionType.Refill;
    public override Bill Bill => SenderBill;

    public override void Execute()
    {
        SenderBill.Balance += Summary;
    }

    public override void Cancel()
    {
        SenderBill.Balance -= Summary;
    }
}