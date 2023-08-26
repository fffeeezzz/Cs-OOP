using Banks.Models.Bills;
using Banks.Tools;

namespace Banks.Models.Transactions;

public class WithdrawTransaction : Transaction
{
    public WithdrawTransaction(decimal summary, Bill bill)
        : base(summary)
    {
        SetDestinationBill(bill);
    }

    public override TransactionType Type => TransactionType.Withdraw;
    public override Bill Bill => DestinationBill;

    public override void Execute()
    {
        if (DestinationBill.BillType() == BillType.Deposit &&
            (Bank.DepositTerm.Equals(TimeSpan.MinValue) ||
             DateTime.Now.Ticks - SenderBill.CreatedAt < Bank.DepositTerm.Ticks))
        {
            throw BanksExceptionCollection.DepositTermException(nameof(Bank.DepositTerm));
        }

        DestinationBill.Balance -= Summary;
    }

    public override void Cancel()
    {
        DestinationBill.Balance += Summary;
    }
}