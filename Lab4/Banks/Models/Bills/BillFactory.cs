namespace Banks.Models.Bills;

public static class BillFactory
{
    public static Bill CreateBill(BillType billType)
    {
        return billType switch
        {
            BillType.Debit => new DebitBill(),
            BillType.Deposit => new DepositBill(),
            BillType.Credit => new CreditBill(),
            _ => throw new ArgumentOutOfRangeException(nameof(billType), billType, null)
        };
    }
}