using Shops.Tools;

namespace Shops.Models;

public class Customer
{
    private const decimal MinAllowableMoneyAmount = 0;
    private readonly ProductCollection _productCollection;

    public Customer(string name, decimal moneyAmount, int id)
    {
        Validate(name, moneyAmount);

        Name = name;
        MoneyAmount = moneyAmount;
        Id = id;
        _productCollection = new ProductCollection();
    }

    public string Name { get; }
    public decimal MoneyAmount { get; private set; }
    public int Id { get; }

    public Customer AddProducts(params Product[] products)
    {
        _productCollection.AddProducts(products);

        return this;
    }

    public Product FindProductById(Guid id)
    {
        return _productCollection.FindProductById(id);
    }

    public void DecreaseMoneyAmount(decimal money)
    {
        if (MoneyAmount < money)
        {
            throw ShopManagerExceptionCollection.IsLessException(nameof(MoneyAmount));
        }

        MoneyAmount -= money;
    }

    public int CountOfProducts() => _productCollection.CountOfProducts();

    private void Validate(string name, decimal moneyAmount)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw ShopManagerExceptionCollection.IsBlankOrNullException(nameof(name));
        }

        if (moneyAmount < MinAllowableMoneyAmount)
        {
            throw ShopManagerExceptionCollection.IsLessException(nameof(moneyAmount));
        }
    }
}