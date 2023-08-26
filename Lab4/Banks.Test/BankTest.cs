using Banks.Models;
using Banks.Models.Bills;
using Banks.Models.Transactions;
using Banks.Services;
using Banks.Test.Attributes;
using Banks.Tools;
using Xunit;

namespace Banks.Test;

[TestCaseOrderer("Bank.Test.Orderers.PriorityOrderer", "Bank.Test")]
public class BankTest
{
    private CentralBank _centralBank = CentralBank.GetInstance();

    [Fact]
    [TestPriority(1)]
    public void TestRegisterClientsAccount_AccountHasBeenRegisteredAndClientIsDoubtful()
    {
        _centralBank = CentralBank.Reset();
        Bank vtbBank = _centralBank.AddBank(Fixtures.VtbBankName);

        _centralBank.RegisterAccount(vtbBank.Name, Fixtures.DoubtfulClient_1);
        Account account = _centralBank.GetClientsAccountById(Fixtures.DoubtfulClient_1.Id.ToString());

        Assert.True(account.Client.IsDoubtful);
    }

    [Fact]
    [TestPriority(2)]
    public void TestRefillTransaction_ClientsBalanceHasBeenRefilled()
    {
        _centralBank = CentralBank.Reset();
        _centralBank.AddBank(Fixtures.VtbBankName);
        _centralBank.RegisterAccount(Fixtures.VtbBankName, Fixtures.ValidClient_1);
        Account account = _centralBank.GetClientsAccountById(Fixtures.ValidClient_1.Id.ToString());
        Bill bill = _centralBank.AddBill(account, BillType.Debit);

        _centralBank.ExecuteTransaction(new RefillTransaction(Fixtures.RefillMoney, bill));

        Assert.Equal(Fixtures.RefillMoney, bill.Balance);
    }

    [Fact]
    [TestPriority(3)]
    public void TestWithdrawTransactionByValidClient_ClientsBalanceHasBeenDecreased()
    {
        _centralBank = CentralBank.Reset();
        _centralBank.AddBank(Fixtures.VtbBankName);
        _centralBank.RegisterAccount(Fixtures.VtbBankName, Fixtures.ValidClient_1);
        Account account = _centralBank.GetClientsAccountById(Fixtures.ValidClient_1.Id.ToString());
        Bill bill = _centralBank.AddBill(account, BillType.Debit);
        _centralBank.ExecuteTransaction(new RefillTransaction(Fixtures.RefillMoney, bill));

        _centralBank.ExecuteTransaction(new WithdrawTransaction(Fixtures.WithdrawMoney, bill));
        decimal expectedBalance = Fixtures.RefillMoney - Fixtures.WithdrawMoney;

        Assert.Equal(expectedBalance, bill.Balance);
    }

    [Fact]
    [TestPriority(4)]
    public void TestTransferTransactionByValidClient_BalancesHasBeenChanged()
    {
        _centralBank = CentralBank.Reset();
        _centralBank.AddBank(Fixtures.VtbBankName);
        _centralBank.RegisterAccount(Fixtures.VtbBankName, Fixtures.ValidClient_1);
        _centralBank.AddBank(Fixtures.SberBankName);
        _centralBank.RegisterAccount(Fixtures.SberBankName, Fixtures.ValidClient_2);
        Account account1 = _centralBank.GetClientsAccountById(Fixtures.ValidClient_1.Id.ToString());
        Account account2 = _centralBank.GetClientsAccountById(Fixtures.ValidClient_2.Id.ToString());
        Bill bill1 = _centralBank.AddBill(account1, BillType.Debit);
        Bill bill2 = _centralBank.AddBill(account2, BillType.Debit);
        _centralBank.ExecuteTransaction(new RefillTransaction(Fixtures.RefillMoney, bill1));

        _centralBank.ExecuteTransaction(new TransferTransaction(Fixtures.TransferMoney, bill1, bill2));
        decimal expectedBalanceAtBill1 = Fixtures.RefillMoney - Fixtures.TransferMoney;
        decimal expectedBalanceAtBill2 = Fixtures.TransferMoney;

        Assert.Equal(expectedBalanceAtBill1, bill1.Balance);
        Assert.Equal(expectedBalanceAtBill2, bill2.Balance);
    }

    [Fact]
    [TestPriority(5)]
    public void TestWithdrawTransactionByDoubtfulClient_ThrowDoubtfulLimitException()
    {
        _centralBank = CentralBank.Reset();
        Bank vtbBank = _centralBank.AddBank(Fixtures.VtbBankName);
        _centralBank.RegisterAccount(vtbBank.Name, Fixtures.DoubtfulClient_1);
        Account account = _centralBank.GetClientsAccountById(Fixtures.DoubtfulClient_1.Id.ToString());
        Bill bill = _centralBank.AddBill(account, BillType.Debit);
        _centralBank.ExecuteTransaction(new RefillTransaction(Fixtures.RefillMoney, bill));
        _centralBank.ChangeDoubtfulLimit(Fixtures.VtbBankName, 10000);

        Assert.Throws<BanksException>(() =>
            _centralBank.ExecuteTransaction(new WithdrawTransaction(Fixtures.WithdrawMoney, bill)));
    }

    [Fact]
    [TestPriority(6)]
    public void TestWithdrawTransactionByDoubtfulClient_ThrowCreditLimitException()
    {
        _centralBank = CentralBank.Reset();
        Bank vtbBank = _centralBank.AddBank(Fixtures.VtbBankName);
        _centralBank.RegisterAccount(vtbBank.Name, Fixtures.DoubtfulClient_1);
        Account account = _centralBank.GetClientsAccountById(Fixtures.DoubtfulClient_1.Id.ToString());
        Bill bill = _centralBank.AddBill(account, BillType.Credit);
        _centralBank.ExecuteTransaction(new RefillTransaction(Fixtures.RefillMoney, bill));
        _centralBank.ChangeCreditLimit(Fixtures.VtbBankName, -10000);

        Assert.Throws<BanksException>(() =>
            _centralBank.ExecuteTransaction(new WithdrawTransaction(Fixtures.WithdrawMoney, bill)));
    }

    [Fact]
    [TestPriority(7)]
    public void TestCancelTransaction_TransactionSuccessfullyCanceled()
    {
        _centralBank = CentralBank.Reset();
        _centralBank.AddBank(Fixtures.VtbBankName);
        _centralBank.RegisterAccount(Fixtures.VtbBankName, Fixtures.ValidClient_1);
        _centralBank.AddBank(Fixtures.SberBankName);
        _centralBank.RegisterAccount(Fixtures.SberBankName, Fixtures.ValidClient_2);
        Account account1 = _centralBank.GetClientsAccountById(Fixtures.ValidClient_1.Id.ToString());
        Account account2 = _centralBank.GetClientsAccountById(Fixtures.ValidClient_2.Id.ToString());
        Bill bill1 = _centralBank.AddBill(account1, BillType.Debit);
        Bill bill2 = _centralBank.AddBill(account2, BillType.Debit);
        _centralBank.ExecuteTransaction(new RefillTransaction(Fixtures.RefillMoney, bill1));
        Transaction transaction = new TransferTransaction(Fixtures.TransferMoney, bill1, bill2);
        _centralBank.ExecuteTransaction(new TransferTransaction(Fixtures.TransferMoney, bill1, bill2));

        _centralBank.CancelTransaction(transaction);
        decimal expectedBalanceAtBill1 = Fixtures.RefillMoney;
        decimal expectedBalanceAtBill2 = decimal.Zero;

        Assert.Equal(expectedBalanceAtBill1, bill1.Balance);
        Assert.Equal(expectedBalanceAtBill2, bill2.Balance);
    }

    [Fact]
    [TestPriority(8)]
    public void TestBankPaysAndTakesCommissions_BalancesHasBeenUpdated()
    {
        _centralBank = CentralBank.Reset();
        _centralBank.AddBank(Fixtures.VtbBankName);
        _centralBank.RegisterAccount(Fixtures.VtbBankName, Fixtures.ValidClient_1);
        _centralBank.RegisterAccount(Fixtures.VtbBankName, Fixtures.ValidClient_2);
        Account account1 = _centralBank.GetClientsAccountById(Fixtures.ValidClient_1.Id.ToString());
        Account account2 = _centralBank.GetClientsAccountById(Fixtures.ValidClient_2.Id.ToString());
        Bill bill1 = _centralBank.AddBill(account1, BillType.Debit);
        Bill bill2 = _centralBank.AddBill(account2, BillType.Debit);
        _centralBank.ExecuteTransaction(new RefillTransaction(Fixtures.RefillMoney, bill1));
        Transaction transaction = new TransferTransaction(Fixtures.TransferMoney, bill1, bill2);
        _centralBank.ExecuteTransaction(new TransferTransaction(Fixtures.TransferMoney, bill1, bill2));
        _centralBank.ChangeDebitPercent(Fixtures.VtbBankName, Fixtures.DebitPercent);

        decimal expectedSaving1 = bill1.Savings + ((Fixtures.DebitPercent * bill1.Balance) / 100);
        decimal expectedSaving2 = bill2.Savings + ((Fixtures.DebitPercent * bill2.Balance) / 100);
        expectedSaving1 += (Fixtures.DebitPercent * bill1.Balance) / 100;
        expectedSaving2 += (Fixtures.DebitPercent * bill2.Balance) / 100;
        _centralBank.SkipDays(60);

        Assert.Equal(expectedSaving1, bill1.Savings);
        Assert.Equal(expectedSaving2, bill2.Savings);
    }
}