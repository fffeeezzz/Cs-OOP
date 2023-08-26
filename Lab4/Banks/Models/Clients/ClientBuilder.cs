namespace Banks.Models.Clients;

public class ClientBuilder : IClientBuilder
{
    private string _name;
    private string _surname;
    private string _address;
    private string _passportSeries;

    public IClientBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public IClientBuilder SetSurname(string surname)
    {
        _surname = surname;
        return this;
    }

    public IClientBuilder SetAddress(string address)
    {
        _address = address;
        return this;
    }

    public IClientBuilder SetPassportSeries(string series)
    {
        _passportSeries = series;
        return this;
    }

    public Client Create()
    {
        var client = new Client(_name, _surname, _address, _passportSeries);
        Reset();

        return client;
    }

    private void Reset()
    {
        _name = null;
        _surname = null;
        _address = null;
        _passportSeries = null;
    }
}