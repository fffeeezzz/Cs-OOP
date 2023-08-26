using Banks.Models.Clients;

namespace Banks.Observer;

public interface ISubject
{
    void Subscribe(Client client);

    void Notify(string updatedValue, decimal value);
}