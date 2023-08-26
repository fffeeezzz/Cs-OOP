namespace Banks.Models.Bills;

public class DebitBill : Bill
{
    public override BillType BillType() => Bills.BillType.Debit;

    internal override void PaySaving()
    {
        Balance += Savings;
    }
}