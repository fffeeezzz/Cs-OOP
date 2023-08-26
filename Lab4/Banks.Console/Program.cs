using Banks.Services;

namespace Banks.Console;

public static class Program
{
    public static void Main()
    {
        var console = new ConsoleUI(CentralBank.GetInstance());
        console.Start();
    }
}