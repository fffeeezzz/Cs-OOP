using System.Net.Mime;
using System.Text;
using Banks.Models;
using Banks.Models.Bills;
using Banks.Models.Clients;
using Banks.Models.Transactions;
using Banks.Services;
using Banks.Tools;

namespace Banks.Console;

public class ConsoleUI
{
    private readonly Dictionary<string, Action> _commands;
    private readonly CentralBank _centralBank;

    public ConsoleUI(CentralBank centralBank)
    {
        _commands = new Dictionary<string, Action>();
        _centralBank = centralBank ?? throw BanksExceptionCollection.IsNullException(nameof(centralBank));
    }

    public void Start()
    {
        _commands.Add("1", CreateBank);
        _commands.Add("2", ChangeCondition);
        _commands.Add("3", CreateClient);
        _commands.Add("4", ChangeClient);
        _commands.Add("5", CreateBill);
        _commands.Add("6", ShowBills);
        _commands.Add("7", ExecuteTransaction);
        _commands.Add("8", Balance);
        _commands.Add("9", ShowUpdates);
        _commands.Add("10", Subscribe);
        _commands.Add("20", ShowBanks);
        _commands.Add("21", ShowClients);
        Menu();

        string line = System.Console.ReadLine();
        while (line != "0")
        {
            if (_commands.ContainsKey(line ?? throw new BanksException("invalid argument")))
            {
                _commands[line]();
            }
            else
            {
                System.Console.WriteLine("Command not found");
                System.Console.Write("If you dont want to try again enter N/n: ");
                string choice = System.Console.ReadLine();
                switch (choice)
                {
                    case "n":
                    case "N":
                        return;
                }
            }

            Menu();
            line = System.Console.ReadLine();
        }
    }

    public void Menu()
    {
        System.Console.Clear();

        var stringBuilder = new StringBuilder();
        stringBuilder
            .AppendFormat("-------------------------------Bank System--------------------------------------\n")
            .AppendFormat("Choose an action 1-9\n")
            .AppendFormat("1. Create a bank\n")
            .AppendFormat("2. Change a condition of bank\n")
            .AppendFormat("3. Create a client\n")
            .AppendFormat("4. Change a client\n")
            .AppendFormat("5. Create a bill\n")
            .AppendFormat("6. Show clients bills\n")
            .AppendFormat("7. Execute transaction\n")
            .AppendFormat("8. Show balance\n")
            .AppendFormat("9. Show updates\n")
            .AppendFormat("10. Subscribe client\n")
            .AppendFormat("20. Show banks\n")
            .AppendFormat("21. Show clients\n")
            .AppendFormat("0. Exit\n\n")
            .AppendFormat("Action: ");

        System.Console.Write(stringBuilder.ToString());
    }

    public void CreateBank()
    {
        string bankName = Reader.Input("Bank name");
        try
        {
            _centralBank.AddBank(bankName);
        }
        catch (Exception e)
        {
            System.Console.WriteLine(e.Message);
            System.Console.Write("If you want to continue enter Y/y, if you want to exit enter N/n: ");
            string choice = System.Console.ReadLine();
            switch (choice)
            {
                case "y":
                case "Y":
                    return;
                case "n":
                case "N":
                    throw;
            }
        }

        System.Console.WriteLine("Bank has been created.");
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    public void ChangeCondition()
    {
        if (!_centralBank.IsBanksExist())
        {
            System.Console.WriteLine("Banks dont exist. Please create banks.");
            System.Console.WriteLine("Enter any symbol to continue.");
            System.Console.ReadLine();
            return;
        }

        ShowInfoAboutBanks();
        string bankName = System.Console.ReadLine();
        System.Console.WriteLine("Choose option 1-5:");
        System.Console.WriteLine("1. Credit limit\n" +
                                 "2. Credit commission\n" +
                                 "3. Debit percent\n" +
                                 "4. Deposit term\n" +
                                 "5. Limit for operations of doubtfull person:");

        string option = System.Console.ReadLine();
        string value = Reader.Input("Value to change");

        switch (option)
        {
            case "1":
                _centralBank.ChangeCreditLimit(bankName, Convert.ToDecimal(value));
                break;
            case "2":
                _centralBank.ChangeCreditCommission(bankName, Convert.ToDecimal(value));
                break;
            case "3":
                _centralBank.ChangeDebitPercent(bankName, Convert.ToDecimal(value));
                break;
            case "4":
                System.Console.WriteLine("Format: ");
                System.Console.WriteLine("Count days");
                _centralBank.ChangeDepositTerm(
                    bankName,
                    new TimeSpan(Convert.ToInt32(value), 0, 0, 0));
                break;
            case "5":
                _centralBank.ChangeDoubtfulLimit(bankName, Convert.ToDecimal(value));
                break;
            default:
                System.Console.WriteLine("Option not found");
                System.Console.Write("If you want to continue enter Y/y, if you want to exit enter N/n: ");
                string choice = System.Console.ReadLine();
                switch (choice)
                {
                    case "y":
                    case "Y":
                        return;
                    case "n":
                    case "N":
                        throw new ArgumentOutOfRangeException();
                }

                break;
        }

        System.Console.WriteLine("Condition has been changed.");
        System.Console.WriteLine("Enter any symbol to continue.");
    }

    public void CreateClient()
    {
        if (!_centralBank.IsBanksExist())
        {
            System.Console.WriteLine("Banks dont exist. Please create banks.");
            System.Console.WriteLine("Enter any symbol to continue.");
            System.Console.ReadLine();
            return;
        }

        ShowInfoAboutBanks();
        string bankName = System.Console.ReadLine();
        Client client = Reader.ClientInput();
        _centralBank.RegisterAccount(bankName, client);

        Account account = _centralBank.GetClientsAccountById(_centralBank.GetBank(bankName), client.Id);

        ShowClient(account.Client);
        System.Console.WriteLine("Client has been created.");
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    public void ChangeClient()
    {
        System.Console.WriteLine("Which client you want to change?\n");
        ShowClients();
        System.Console.Write("Enter clients ID: ");
        string id = System.Console.ReadLine();
        Account account = _centralBank.GetClientsAccountById(id);
        Client client = Reader.ClientInput();
        account.ChangeClient(client);

        ShowClient(account.Client);
        System.Console.WriteLine("Client has been changed.");
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    public void CreateBill()
    {
        System.Console.WriteLine("For which client you want to create a bill?\n");
        ShowClients();
        string bankName = System.Console.ReadLine();
        System.Console.Write("Enter clients ID: ");
        string id = System.Console.ReadLine();
        System.Console.Write("Enter bank name which contains this client ID: ");
        Account account = _centralBank.GetClientsAccountById(id);

        System.Console.WriteLine("Conditions:\n" + _centralBank.BanksConditions(bankName));
        System.Console.WriteLine("What type of bill you choose?");
        System.Console.WriteLine("1. Debit");
        System.Console.WriteLine("2. Deposit");
        System.Console.WriteLine("3. Credit");
        System.Console.Write("Enter type: ");
        string billType = System.Console.ReadLine();
        BillType type = billType switch
        {
            "1" => BillType.Debit,
            "2" => BillType.Deposit,
            "3" => BillType.Credit,
            _ => throw new BanksException("This type not found")
        };

        _centralBank.AddBill(account, type);
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    public void ShowBills()
    {
        System.Console.WriteLine("Which clients bills you wanna see\n");
        ShowClients();
        System.Console.Write("Enter clients ID: ");
        string id = System.Console.ReadLine();
        Account account = _centralBank.GetClientsAccountById(id);
        System.Console.Write("In which bank? Enter bank name: ");
        string bankName = System.Console.ReadLine();

        System.Console.WriteLine(_centralBank.ClientsBills(bankName, account.Client));
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    public void ExecuteTransaction()
    {
        System.Console.WriteLine("Choose type of transaction:");
        System.Console.WriteLine("1. Transfer money to another bill\n" +
                                 "2. Refill money to bill\n" +
                                 "3. Withdraw money bill");
        System.Console.Write("Enter your choice: ");
        string type = System.Console.ReadLine();
        switch (type)
        {
            case "1":
                ExecuteTransfer();
                break;

            case "2":
                ExecuteRefill();
                break;

            case "3":
                ExecuteWithdraw();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Balance()
    {
        System.Console.WriteLine("Choose a client bill to check balance");
        ShowBills();
        System.Console.Write("Enter bills ID: ");
        var billId = Guid.Parse(System.Console.ReadLine() ?? string.Empty);

        _centralBank.BillBalance(billId);
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    public void ShowUpdates()
    {
        System.Console.WriteLine("-------------------------Updates----------------------");
        _centralBank.ShowUpdates();
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    public void Subscribe()
    {
        System.Console.WriteLine("Choose client to subscribe");
        ShowClients();
        System.Console.Write("Enter clients ID: ");
        string id = System.Console.ReadLine();
        System.Console.Write("Enter bank name which contains this client ID: ");
        Account account = _centralBank.GetClientsAccountById(id);
        System.Console.Write("In which bank? Enter bank name: ");
        string bankName = System.Console.ReadLine();

        _centralBank.Subscribe(account.Client, bankName);
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    public void ShowBanks()
    {
        ShowInfoAboutBanks();
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    public void ShowClients()
    {
        System.Console.WriteLine(_centralBank.Clients());
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    private void ShowInfoAboutBanks()
    {
        System.Console.WriteLine(
            "-------------------------------Banks Information--------------------------------------");
        System.Console.WriteLine(_centralBank.AllBanks());
        System.Console.Write("Choose bank: ");
    }

    private void ExecuteTransfer()
    {
        System.Console.WriteLine("Choose a sender client bill");
        ShowBills();
        System.Console.Write("Enter bills ID: ");
        var senderBillId = Guid.Parse(System.Console.ReadLine() ?? string.Empty);
        Bill senderBill = _centralBank.GetBillById(senderBillId);

        System.Console.WriteLine("Choose a destination client bill");
        ShowBills();
        System.Console.Write("Enter bills ID: ");
        var destinationBillId = Guid.Parse(System.Console.ReadLine() ?? string.Empty);
        Bill destinationBill = _centralBank.GetBillById(destinationBillId);

        System.Console.Write("Enter money to transfer: ");
        decimal money = Convert.ToDecimal(System.Console.ReadLine());
        _centralBank.ExecuteTransaction(new TransferTransaction(money, senderBill, destinationBill));

        System.Console.WriteLine("Transaction has been executed.");
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    private void ExecuteRefill()
    {
        System.Console.WriteLine("Choose a client bill");
        ShowBills();
        System.Console.Write("Enter bills ID: ");
        var billId = Guid.Parse(System.Console.ReadLine() ?? string.Empty);
        Bill bill = _centralBank.GetBillById(billId);

        System.Console.Write("Enter money to refill:");
        decimal money = Convert.ToDecimal(System.Console.ReadLine());
        _centralBank.ExecuteTransaction(new RefillTransaction(money, bill));

        System.Console.WriteLine("Transaction has been executed.");
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    private void ExecuteWithdraw()
    {
        System.Console.WriteLine("Choose a client bill");
        ShowBills();
        System.Console.Write("Enter bills ID: ");
        var billId = Guid.Parse(System.Console.ReadLine() ?? string.Empty);
        Bill bill = _centralBank.GetBillById(billId);

        System.Console.Write("Enter money to withdraw:");
        decimal money = Convert.ToDecimal(System.Console.ReadLine());
        _centralBank.ExecuteTransaction(new WithdrawTransaction(money, bill));

        System.Console.WriteLine("Transaction has been executed.");
        System.Console.WriteLine("Enter any symbol to continue.");
        System.Console.ReadLine();
    }

    private void ShowClient(Client client)
    {
        System.Console.WriteLine("\n-----------------------------New Client-----------------------------");
        System.Console.WriteLine($"Id       - {client.Id}\n" +
                                 $"Name     - {client.Name}\n" +
                                 $"Surname  - {client.Surname}");
        if (client.Address is not null)
        {
            System.Console.WriteLine($"Address  - {client.Address}");
        }

        if (client.PassportSeries != 0)
        {
            System.Console.WriteLine($"Passport - {client.PassportSeries}");
        }

        System.Console.WriteLine("-----------------------------New Client-----------------------------\n");
    }
}