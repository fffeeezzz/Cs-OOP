using Banks.Models.Bills;
using Banks.Models.Clients;

namespace Banks.Test;

public static class Fixtures
{
    private static readonly IClientBuilder ClientBuilder = new ClientBuilder();

    public static Client ValidClient_1 { get; } = ClientBuilder
        .SetName("Denis")
        .SetSurname("Portnov")
        .SetAddress("Kislovodsk")
        .SetPassportSeries("7777")
        .Create();

    public static Client ValidClient_2 { get; } = ClientBuilder
        .SetName("Muzika")
        .SetSurname("Myzikus")
        .SetAddress("Perm")
        .SetPassportSeries("6666")
        .Create();

    public static Client DoubtfulClient_1 { get; } = ClientBuilder
        .SetName("Doubtful 1")
        .SetSurname("Doubtful 1")
        .Create();

    public static Client DoubtfulClient_2 { get; } = ClientBuilder
        .SetName("Doubtful 2")
        .SetSurname("Doubtful 2")
        .Create();

    public static TimeSpan DepositTerm { get; } = new TimeSpan(60, 0, 0, 0);

    public static string VtbBankName { get; } = "vtb";
    public static string SberBankName { get; } = "sberbank";

    public static List<DepositPercent> VtbPercents { get; } = new List<DepositPercent>
    {
        new DepositPercent(decimal.Zero, 0.01m),
        new DepositPercent(50000, 0.02m),
        new DepositPercent(100000, 0.05m),
    };

    public static decimal DebitPercent { get; } = 50;

    public static decimal RefillMoney { get; } = 1000000;
    public static decimal TransferMoney { get; } = 45000;
    public static decimal WithdrawMoney { get; } = 75000;
}