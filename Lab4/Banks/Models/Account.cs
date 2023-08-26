using Banks.Models.Bills;
using Banks.Models.Clients;
using Banks.Tools;

namespace Banks.Models;

public class Account : IEquatable<Account>
{
    private readonly List<Bill> _bills;

    public Account(Client client)
    {
        Client = client ?? throw BanksExceptionCollection.IsNullException(nameof(client));
        _bills = new List<Bill>();
    }

    public Client Client { get; private set; }

    public IReadOnlyCollection<Bill> Bills => _bills;

    public void ChangeClient(Client client)
    {
        if (client is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(client));
        }

        Client = client;
    }

    public void AddBill(Bill bill)
    {
        if (bill is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(bill));
        }

        _bills.Add(bill);
    }

    public override string ToString()
        => _bills.Aggregate<Bill, string>(null, (current, bill)
            => current + $"Bill type - {bill.BillType()}; ID - {bill.Id}\n");

    public bool Equals(Account other)
    {
        return other != null && Equals(Client, other.Client);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Account);
    }

    public override int GetHashCode()
    {
        return Client.GetHashCode();
    }
}