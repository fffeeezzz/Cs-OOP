using Banks.Models.Clients;

namespace Banks.Console;

public class Reader
{
    private static string _name;
    private static string _surname;
    private static string _address;
    private static string _passportSeries;

    public static string Input(string dataName)
    {
        System.Console.Write($"Enter {dataName}: ");
        string value = System.Console.ReadLine();
        return value;
    }

    public static string OptionalInput(string dataName)
    {
        System.Console.WriteLine($"Do you want indicate your {dataName}? Y/n");
        string choice = System.Console.ReadLine();
        switch (choice)
        {
            case "y":
            case "Y":
                return Input(dataName);
            case "n":
            case "N":
                return null;
            default:
                System.Console.WriteLine("Please, try again");
                throw new ArgumentOutOfRangeException();
        }
    }

    public static Client ClientInput()
    {
        System.Console.WriteLine("Enter your personal data");
        _name = Input("Name");

        _surname = Input("Surname");

        System.Console.WriteLine("If you want to use your account in full,\n" +
                                 " you need to provide your address and passport number");

        _address = OptionalInput("Address");
        _passportSeries = OptionalInput("Passport number");

        return CreatePerson();
    }

    private static Client CreatePerson()
    {
        IClientBuilder clientBuilder = new ClientBuilder();
        Client client = clientBuilder
            .SetName(_name)
            .SetSurname(_surname)
            .SetAddress(_address)
            .SetPassportSeries(_passportSeries)
            .Create();

        Reset();
        return client;
    }

    private static void Reset()
    {
        _name = null;
        _surname = null;
        _address = null;
        _passportSeries = null;
    }
}