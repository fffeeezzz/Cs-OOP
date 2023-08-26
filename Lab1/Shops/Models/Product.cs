using Shops.Tools;

namespace Shops.Models;

public class Product
{
    private const decimal MinAllowablePrimeCoast = 0;

    public Product(string name, decimal primeCoast)
    {
        Validate(name, primeCoast);

        Id = Guid.NewGuid();
        Name = name;
        PrimeCoast = primeCoast;
    }

    public Guid Id { get; }
    public string Name { get; }
    public decimal PrimeCoast { get; private set; }

    public void ChangePrimeCoast(decimal coast)
    {
        if (coast < MinAllowablePrimeCoast)
        {
            throw ShopManagerExceptionCollection.IsLessException(nameof(coast));
        }

        PrimeCoast = coast;
    }

    private void Validate(string name, decimal primeCoast)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw ShopManagerExceptionCollection.IsBlankOrNullException(nameof(name));
        }

        if (primeCoast < MinAllowablePrimeCoast)
        {
            throw ShopManagerExceptionCollection.IsLessException(nameof(primeCoast));
        }
    }
}