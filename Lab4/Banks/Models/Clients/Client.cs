using System.Text;
using Banks.Observer;
using Banks.Tools;

namespace Banks.Models.Clients;

public class Client : IEquatable<Client>, IObserver
{
    private const int ValidLenOfPassportSeries = 4;
    private readonly List<string> _updates;

    public Client(string name, string surname, string address, string passportSeries)
    {
        Validate(name, surname);

        Id = Guid.NewGuid();
        Name = name;
        Surname = surname;
        Address = address;
        if (passportSeries is not null)
        {
            EnsurePassportSeriesIsValid(passportSeries);
            PassportSeries = int.Parse(passportSeries);
        }

        _updates = new List<string>();
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Surname { get; }
    public string Address { get; }
    public int PassportSeries { get; }

    public bool IsDoubtful => string.IsNullOrWhiteSpace(Address) || PassportSeries == 0;

    public void ShowUpdates()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder
            .AppendFormat($"-------------------Client with name - {Name} and ID - {Id} ------------------- \n");

        foreach (string update in _updates)
        {
            stringBuilder.AppendFormat(update);
        }

        Console.WriteLine(stringBuilder.ToString());
    }

    public void Update(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw BanksExceptionCollection.IsNullException(nameof(message));
        }

        _updates.Add(message);
    }

    public bool Equals(Client other)
    {
        return other != null && Id.Equals(other.Id);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Client);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    private void Validate(string name, string surname)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw BanksExceptionCollection.IsNullException(nameof(name));
        }

        if (string.IsNullOrWhiteSpace(surname))
        {
            throw BanksExceptionCollection.IsNullException(nameof(surname));
        }
    }

    private void EnsurePassportSeriesIsValid(string passportSeries)
    {
        if (string.IsNullOrWhiteSpace(passportSeries))
        {
            throw BanksExceptionCollection.IsNullException(nameof(passportSeries));
        }

        if (passportSeries.Length != ValidLenOfPassportSeries)
        {
            throw BanksExceptionCollection.IsLongException(passportSeries);
        }

        if (passportSeries.Any(c => !char.IsDigit(c)))
        {
            throw BanksExceptionCollection.IsDigitException(nameof(passportSeries));
        }
    }
}