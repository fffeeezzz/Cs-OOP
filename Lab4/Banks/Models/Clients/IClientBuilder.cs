namespace Banks.Models.Clients;

public interface IClientBuilder
{
    IClientBuilder SetName(string name);

    IClientBuilder SetSurname(string surname);

    IClientBuilder SetAddress(string address);

    IClientBuilder SetPassportSeries(string series);

    Client Create();
}