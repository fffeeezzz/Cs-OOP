namespace Banks.Tools;

public class BanksExceptionCollection
{
    public static BanksException IsNullException(string paramName)
        => new BanksException("This value should not be null.", paramName);

    public static BanksException IsLowException(string paramName)
        => new BanksException("This value is too low.", paramName);

    public static BanksException IsHighException(string paramName)
        => new BanksException("This value is too high.", paramName);

    public static BanksException IsLongException(string paramName)
        => new BanksException("This value is too long.", paramName);

    public static BanksException IsDigitException(string paramName)
        => new BanksException("This value should contains only digits.", paramName);

    public static BanksException DepositTermException(string paramName)
        => new BanksException("Deposit term does not finished.", paramName);

    public static BanksException CreditLimitException(string paramName)
        => new BanksException("Credit limit has been reached.", paramName);

    public static BanksException DoubtfulLimitException(string paramName)
        => new BanksException("Doubtful limit has been reached.", paramName);

    public static BanksException BankDoesNotExistException(string paramName)
        => new BanksException("Bank does not exist.", paramName);

    public static BanksException AccountDoesNotExistException(string paramName)
        => new BanksException("Account does not exist.", paramName);

    public static BanksException BillDoesNotExistException(string paramName)
        => new BanksException("Bill does not exist.", paramName);

    public static BanksException BankIsNotUniqueException(string paramName)
        => new BanksException("This bank is already exist.", paramName);

    public static BanksException TransactionDoesNotExistException(string paramName)
        => new BanksException("Transaction does not exist.", paramName);
}