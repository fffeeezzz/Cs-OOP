using System.Runtime.CompilerServices;
using System.Text;
using Banks.Models.Bills;
using Banks.Models.Clients;
using Banks.Observer;
using Banks.Tools;

namespace Banks.Models;

public class Bank : ISubject
{
    private readonly List<Account> _accounts;
    private readonly List<Client> _subscribers;
    private readonly List<DepositPercent> _depositPercent;

    public Bank(string name)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw BanksExceptionCollection.IsNullException(nameof(name));

        CreditLimit = decimal.Zero;
        CreditCommission = decimal.Zero;
        DebitPercent = decimal.Zero;
        DepositTerm = TimeSpan.MinValue;

        _accounts = new List<Account>();
        _depositPercent = new List<DepositPercent>();
        _subscribers = new List<Client>();
    }

    public Guid Id { get; }
    public string Name { get; }
    public decimal CreditLimit { get; private set; }
    public decimal CreditCommission { get; private set; }
    public decimal DebitPercent { get; private set; }
    public TimeSpan DepositTerm { get; private set; }
    public decimal DoubtfulLimit { get; private set; }

    public IReadOnlyList<Account> Accounts => _accounts;
    public IReadOnlyCollection<DepositPercent> DepositPercent => _depositPercent;

    public void SetCreditLimit(decimal limit)
    {
        if (limit > decimal.Zero)
        {
            throw BanksExceptionCollection.IsHighException(nameof(limit));
        }

        CreditLimit = limit;
        Notify("Credit limit", limit);
    }

    public void SetCreditCommission(decimal commission)
    {
        if (commission < decimal.Zero)
        {
            throw BanksExceptionCollection.IsLowException(nameof(commission));
        }

        CreditCommission = commission;
        Notify("Credit commission", commission);
    }

    public void SetDebitPercent(decimal percent)
    {
        if (percent < decimal.Zero)
        {
            throw BanksExceptionCollection.IsLowException(nameof(percent));
        }

        DebitPercent = percent;
        Notify("Debit percent", percent);
    }

    public void SetDepositTerm(TimeSpan term)
    {
        int difference = term.Days - DepositTerm.Days;
        DepositTerm = term;
        Notify("Deposit term", difference);
    }

    public void SetDoubtfulLimit(decimal limit)
    {
        if (limit < decimal.Zero)
        {
            throw BanksExceptionCollection.IsLowException(nameof(limit));
        }

        DoubtfulLimit = limit;
        Notify("Doubtful limit", limit);
    }

    public void AddAccount(Account account)
    {
        if (account is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(account));
        }

        _accounts.Add(account);
    }

    public void AddDepositPercent(DepositPercent depositPercent)
    {
        if (depositPercent is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(depositPercent));
        }

        _depositPercent.Add(depositPercent);
    }

    public void Subscribe(Client client)
    {
        if (client is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(client));
        }

        _subscribers.Add(client);
    }

    public void Notify(string updatedValue, decimal value)
    {
        foreach (Client client in _subscribers)
        {
            string update = $"Bank {Name} updated {updatedValue} to {value}\n";
            client.Update(update);
        }
    }

    public void ShowUpdates()
    {
        foreach (Client client in _subscribers)
        {
            client.ShowUpdates();
        }
    }

    public void Clients()
    {
        var stringBuilder = new StringBuilder();
        foreach (Account account in _accounts)
        {
            stringBuilder.AppendFormat("------------Client------------\n");
            stringBuilder.AppendFormat($"{account.Client}");
        }
    }

    internal void DoSavings()
    {
        IEnumerable<Bill> bills = _accounts.SelectMany(account => account.Bills);
        foreach (Bill bill in bills)
        {
            switch (bill.BillType())
            {
                case BillType.Deposit:
                    bill.ChangeSavings(bill.Savings + (SolveDepositPercent(bill.Balance) * bill.Balance));
                    break;

                case BillType.Debit:
                    bill.ChangeSavings(bill.Savings + ((DebitPercent * bill.Balance) / 100));
                    break;

                case BillType.Credit:
                    if (bill.Balance >= decimal.Zero)
                    {
                        break;
                    }

                    bill.ChangeSavings(bill.Savings + CreditCommission);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private decimal SolveDepositPercent(decimal billBalance)
    {
        _depositPercent.Sort();
        return _depositPercent.Last(b => billBalance >= b.Summary).Percent;
    }
}