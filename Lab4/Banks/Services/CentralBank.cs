using System.Text;
using Banks.Models;
using Banks.Models.Bills;
using Banks.Models.Clients;
using Banks.Models.Transactions;
using Banks.Time;
using Banks.Tools;

namespace Banks.Services;

public class CentralBank
{
    private static CentralBank _instance;
    private readonly List<Bank> _banks;
    private readonly TransactionsChain _transactionsChain;
    private readonly TimeProvider _timeProvider;

    private CentralBank()
    {
        _banks = new List<Bank>();
        _transactionsChain = new TransactionsChain();
        _timeProvider = new TimeProvider();
    }

    public static CentralBank GetInstance()
    {
        return _instance ??= new CentralBank();
    }

    public static CentralBank Reset()
    {
        return new CentralBank();
    }

    public bool IsBanksExist() => _banks.Any();

    public Bank GetBank(string name)
    {
        return _banks.Find(b => b.Name == name) ??
               throw BanksExceptionCollection.BankDoesNotExistException(nameof(name));
    }

    public void SkipDays(int dayCount)
    {
        _timeProvider.SkipDays(dayCount);
        UpdateSavings(dayCount / 30);
    }

    public void UpdateSavings(int timesToUpdate)
    {
        if (!_timeProvider.IsTimeToUpdate()) return;
        for (int i = 0; i < timesToUpdate; i++)
        {
            foreach (Bank bank in _banks)
            {
                bank.DoSavings();
            }
        }

        _timeProvider.UpdatedAt = _timeProvider.CurrentTime;
    }

    public void Subscribe(Client client, string bankName)
    {
        Bank bank = GetBank(bankName);
        bank.Subscribe(client);
    }

    public Bank AddBank(string name)
    {
        EnsureStringIsNotNull(name);
        EnsureBankIsUnique(name);

        var bank = new Bank(name);
        _banks.Add(bank);

        return bank;
    }

    public void RegisterAccount(string bankName, Client client)
    {
        EnsureStringIsNotNull(bankName);
        EnsureClientIsNotNull(client);

        var account = new Account(client);
        Bank bank = GetBank(bankName);
        bank.AddAccount(account);
    }

    public Bill AddBill(Account account, BillType billType)
    {
        EnsureAccountIsNotNull(account);
        EnsureAccountIsRegistered(account);

        Bill bill = BillFactory.CreateBill(billType);
        account.AddBill(bill);

        return bill;
    }

    public void ChangeCreditLimit(string bankName, decimal limit)
    {
        EnsureStringIsNotNull(bankName);

        Bank bank = GetBank(bankName);
        bank.SetCreditLimit(limit);
    }

    public void ChangeCreditCommission(string bankName, decimal commission)
    {
        EnsureStringIsNotNull(bankName);

        Bank bank = GetBank(bankName);
        bank.SetCreditCommission(commission);
    }

    public void ChangeDebitPercent(string bankName, decimal percent)
    {
        EnsureStringIsNotNull(bankName);

        Bank bank = GetBank(bankName);
        bank.SetDebitPercent(percent);
    }

    public void ChangeDepositTerm(string bankName, TimeSpan timeSpan)
    {
        EnsureStringIsNotNull(bankName);

        Bank bank = GetBank(bankName);
        bank.SetDepositTerm(timeSpan);
    }

    public void ChangeDoubtfulLimit(string bankName, decimal limit)
    {
        EnsureStringIsNotNull(bankName);

        Bank bank = GetBank(bankName);
        bank.SetDoubtfulLimit(limit);
    }

    public void ChangeDepositPercent(string bankName, List<DepositPercent> depositPercents)
    {
        EnsureStringIsNotNull(bankName);
        EnsureDepositPercentIsNotNull(depositPercents);

        Bank bank = GetBank(bankName);
        depositPercents.ForEach(bank.AddDepositPercent);
    }

    public void ExecuteTransaction(Transaction transaction)
    {
        EnsureTransactionIsNotNull(transaction);

        Guid id = transaction.Bill.Id;
        Bank bank = GetBillFromBankById(id);
        Client client = GetClientByBillId(id);

        CheckAbility(client, transaction, bank.DoubtfulLimit);

        transaction.Execute();
        _transactionsChain.PushTransaction(transaction);
    }

    public void CancelTransaction(Transaction transaction)
    {
        _transactionsChain.PopTransaction(transaction);
    }

    public string AllBanks()
    {
        var stringBuilder = new StringBuilder();
        foreach (Bank bank in _banks)
        {
            stringBuilder.AppendFormat(bank.Name + "\n");
        }

        return stringBuilder.ToString();
    }

    public string Clients()
    {
        var stringBuilder = new StringBuilder();
        foreach (Bank bank in _banks)
        {
            stringBuilder.AppendFormat($"-------------------------Bank: {bank.Name}-----------------------------\n");
            foreach (Account account in bank.Accounts)
            {
                stringBuilder.AppendFormat($"Client name - {account.Client.Name}, Client ID - {account.Client.Id}\n");
            }

            stringBuilder.AppendFormat("\n");
        }

        return stringBuilder.ToString();
    }

    public string BanksConditions(string bankName)
    {
        EnsureStringIsNotNull(bankName);

        Bank bank = GetBank(bankName);
        var stringBuilder = new StringBuilder();
        stringBuilder
            .AppendFormat($"Bank name - {bank.Name}")
            .AppendFormat($"Credit limit - {bank.CreditLimit}\n")
            .AppendFormat($"Credit commission - {bank.CreditCommission}\n")
            .AppendFormat($"Debit percent - {bank.DebitPercent}\n")
            .AppendFormat($"Deposit term - {bank.DepositTerm}\n")
            .AppendFormat($"Limit for doubtful clients - {bank.DoubtfulLimit}");

        return stringBuilder.ToString();
    }

    public string ClientsBills(string bankName, Client client)
    {
        Bank bank = GetBank(bankName);
        Account account = GetClientsAccountById(bank, client.Id);

        return account.ToString();
    }

    public Account GetClientsAccountById(Bank bank, Guid id)
    {
        return bank.Accounts.SingleOrDefault(a => a.Client.Id == id) ?? throw new Exception();
    }

    public Account GetClientsAccountById(string id)
    {
        return _banks.SelectMany(b => b.Accounts).SingleOrDefault(a => a.Client.Id == Guid.Parse(id));
    }

    public Bill GetBillById(Guid id)
    {
        return _banks.SelectMany(b => b.Accounts)
                   .SelectMany(a => a.Bills)
                   .SingleOrDefault(bill => bill.Id == id) ??
               throw BanksExceptionCollection.BillDoesNotExistException(nameof(id));
    }

    public void BillBalance(Guid id)
    {
        Console.WriteLine($"Bill balance - {GetBillById(id).Balance}");
    }

    public void ShowUpdates()
    {
        foreach (Bank bank in _banks)
        {
            bank.ShowUpdates();
        }
    }

    private void CheckAbility(Client client, Transaction transaction, decimal limit)
    {
        if (!client.IsDoubtful || transaction.Type is TransactionType.Refill)
        {
            return;
        }

        if (transaction.Summary > limit)
        {
            throw BanksExceptionCollection.DoubtfulLimitException(nameof(limit));
        }
    }

    private Client GetClientByBillId(Guid id)
    {
        return _banks.SelectMany(b => b.Accounts).First(a => a.Bills.Any(b => b.Id == id)).Client
               ?? throw BanksExceptionCollection.AccountDoesNotExistException(nameof(id));
    }

    private Bank GetBillFromBankById(Guid id)
    {
        return _banks.Find(b => b.Accounts.Any(a => a.Bills.Any(bill => bill.Id == id))) ??
               throw BanksExceptionCollection.BillDoesNotExistException(nameof(id));
    }

    private bool IsAccountRegister(Account account)
        => _banks.SelectMany(b => b.Accounts).Any(a => Equals(a, account));

    private void EnsureAccountIsNotNull(Account account)
    {
        if (account is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(account));
        }
    }

    private void EnsureStringIsNotNull(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw BanksExceptionCollection.IsHighException(nameof(value));
        }
    }

    private void EnsureAccountIsRegistered(Account account)
    {
        if (!IsAccountRegister(account))
        {
            throw BanksExceptionCollection.AccountDoesNotExistException(nameof(account));
        }
    }

    private void EnsureClientIsNotNull(Client client)
    {
        if (client is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(client));
        }
    }

    private void EnsureBankIsUnique(string bankName)
    {
        if (_banks.Any(b => b.Name == bankName))
        {
            throw BanksExceptionCollection.BankIsNotUniqueException(nameof(bankName));
        }
    }

    private void EnsureDepositPercentIsNotNull(List<DepositPercent> depositPercents)
    {
        if (depositPercents is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(depositPercents));
        }
    }

    private void EnsureTransactionIsNotNull(Transaction transaction)
    {
        if (transaction is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(transaction));
        }
    }
}