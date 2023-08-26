using System.Runtime.CompilerServices;
using Banks.Models.Bills;
using Banks.Tools;

namespace Banks.Models.Transactions;

public class TransferTransaction : Transaction
{
    public TransferTransaction(decimal summary, Bill senderBill, Bill destinationBill)
        : base(summary)
    {
        SetSenderBill(senderBill);
        SetDestinationBill(destinationBill);
    }

    public override TransactionType Type => TransactionType.Transfer;
    public override Bill Bill => SenderBill;

    public override void Execute()
    {
        switch (SenderBill.BillType())
        {
            case BillType.Deposit:
                if (Bank.DepositTerm.Equals(TimeSpan.MinValue) ||
                    DateTime.Now.Ticks - SenderBill.CreatedAt < Bank.DepositTerm.Ticks)
                {
                    throw BanksExceptionCollection.DepositTermException(nameof(Bank.DepositTerm));
                }

                SenderBill.Balance -= Summary;
                DestinationBill.Balance += Summary;
                break;

            case BillType.Credit:
                if (Bank.CreditLimit < Math.Abs(SenderBill.Balance - Summary))
                {
                    throw BanksExceptionCollection.CreditLimitException(nameof(Bank.CreditLimit));
                }

                SenderBill.Balance -= Summary;
                DestinationBill.Balance += Summary;
                break;

            case BillType.Debit:
                SenderBill.Balance -= Summary;
                DestinationBill.Balance += Summary;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void Cancel()
    {
        SenderBill.Balance += Summary;
        DestinationBill.Balance -= Summary;
    }
}