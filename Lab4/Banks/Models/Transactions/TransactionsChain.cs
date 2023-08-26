using Banks.Tools;

namespace Banks.Models.Transactions;

public class TransactionsChain
{
    private readonly List<Transaction> _transactions;

    public TransactionsChain()
    {
        _transactions = new List<Transaction>();
    }

    internal void PushTransaction(Transaction transaction)
    {
        if (transaction is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(transaction));
        }

        _transactions.Add(transaction);
    }

    internal void PopTransaction(Transaction transaction)
    {
        if (transaction is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(transaction));
        }

        Transaction executedTransaction = _transactions.FirstOrDefault(transaction);
        if (executedTransaction is null)
        {
            throw BanksExceptionCollection.IsNullException(nameof(executedTransaction));
        }

        _transactions.Remove(transaction);
        transaction.Cancel();
    }
}