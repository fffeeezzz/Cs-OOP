namespace Banks.Models.Bills;

public class DepositBill : Bill
{
    public override BillType BillType() => Bills.BillType.Deposit;

    internal override void PaySaving()
    {
        Balance += Savings;
    }
}