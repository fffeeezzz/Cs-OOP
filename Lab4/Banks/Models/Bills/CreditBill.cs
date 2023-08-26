namespace Banks.Models.Bills;

public class CreditBill : Bill
{
    public override BillType BillType() => Bills.BillType.Credit;

    internal override void PaySaving()
    {
        Balance -= Savings;
    }
}