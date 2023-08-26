using Shops.Tools;

namespace Shops.Models;

public class Shop : IEquatable<Shop>
{
    private const decimal MinAllowableMarkup = 0;
    private readonly ProductCollection _productCollection;

    public Shop(string name, string address, decimal markup, int id)
    {
        Validate(name, address, markup);

        Name = name;
        Address = address;
        Markup = markup;
        Id = id;
        Income = decimal.Zero;
        _productCollection = new ProductCollection();
    }

    public string Name { get; }
    public string Address { get; }
    public decimal Markup { get; }
    public int Id { get; }
    public decimal Income { get; private set; }

    public Shop AddProduct(Product product)
    {
        _productCollection.AddProduct(product);

        return this;
    }

    public Shop AddProducts(params Product[] products)
    {
        _productCollection.AddProducts(products);

        return this;
    }

    public void RemoveProduct(Product product)
    {
        _productCollection.RemoveProduct(product);
    }

    public Product FindProductById(Guid id)
    {
        return _productCollection.FindProductById(id);
    }

    public Product FindProductByName(string name)
    {
        return _productCollection.FindProductByName(name);
    }

    public decimal GetProductsSum(params Product[] givenProducts)
    {
        decimal sum = 0;

        foreach (Product givenProduct in givenProducts)
        {
            Product product = FindProductByName(givenProduct.Name);
            if (product is null)
            {
                throw ShopManagerExceptionCollection.IsNullException(nameof(product));
            }

            sum += product.PrimeCoast + (product.PrimeCoast * Markup);
        }

        return sum;
    }

    public void PurchaseProducts(Customer customer, params Product[] givenProducts)
    {
        decimal moneyForProducts = 0;
        var products = new List<Product>();

        foreach (Product givenProduct in givenProducts)
        {
            Product product = FindProductById(givenProduct.Id);
            if (product is null)
            {
                throw ShopManagerExceptionCollection.IsNullException(nameof(product));
            }

            products.Add(product);
        }

        moneyForProducts += products.Sum(x => x.PrimeCoast + (x.PrimeCoast * Markup));
        if (moneyForProducts > customer.MoneyAmount)
        {
            throw ShopManagerExceptionCollection.IsHighException(nameof(moneyForProducts));
        }

        customer.DecreaseMoneyAmount(moneyForProducts);
        customer.AddProducts(products.ToArray());
        products.ForEach(RemoveProduct);
        Income += moneyForProducts;
    }

    public bool ContainsAllProducts(params Product[] givenProducts)
        => givenProducts
            .Select(givenProduct => FindProductByName(givenProduct.Name))
            .All(product => product is not null);

    public bool Equals(Shop other) => other != null && Id == other.Id;
    public override bool Equals(object obj) => Equals(obj as Shop);
    public override int GetHashCode() => Id.GetHashCode();

    public int CountOfProducts() => _productCollection.CountOfProducts();

    private void Validate(string name, string address, decimal markup)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw ShopManagerExceptionCollection.IsBlankOrNullException(nameof(name));
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw ShopManagerExceptionCollection.IsBlankOrNullException(nameof(address));
        }

        if (markup < MinAllowableMarkup)
        {
            throw ShopManagerExceptionCollection.IsLessException(nameof(markup));
        }
    }
}