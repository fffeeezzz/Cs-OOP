using Shops.Tools;

namespace Shops.Models;

public class ProductCollection
{
    private readonly Dictionary<string, List<Product>> _productsByName;

    public ProductCollection()
    {
        _productsByName = new Dictionary<string, List<Product>>();
    }

    public Product AddProduct(Product product)
    {
        EnsureProductIsNotNull(product);

        if (!_productsByName.ContainsKey(product.Name))
        {
            _productsByName.Add(product.Name, new List<Product>());
        }

        _productsByName[product.Name].Add(product);

        return product;
    }

    public IEnumerable<Product> AddProducts(params Product[] givenProducts)
    {
        var products = new List<Product>();
        foreach (Product givenProduct in givenProducts)
        {
            AddProduct(givenProduct);
            products.Add(givenProduct);
        }

        return products;
    }

    public void RemoveProduct(Product product)
    {
        EnsureProductIsNotNull(product);
        EnsureDictionaryContainsKey(product.Name);
        EnsureCollectionContainsValue(product);

        _productsByName[product.Name].Remove(product);
    }

    public Product FindProductById(Guid id)
    {
        return _productsByName.Values.SelectMany(p => p).FirstOrDefault(product => product.Id == id);
    }

    public Product FindProductByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw ShopManagerExceptionCollection.IsBlankOrNullException(nameof(name));
        }

        return _productsByName.Values.SelectMany(p => p).FirstOrDefault(product => product.Name == name);
    }

    public int CountOfProducts()
    {
        return _productsByName.Values.SelectMany(x => x).Count();
    }

    private void EnsureProductIsNotNull(Product product)
    {
        if (product is null)
        {
            throw ShopManagerExceptionCollection.IsNullException(nameof(product));
        }
    }

    private void EnsureDictionaryContainsKey(string key)
    {
        if (!_productsByName.ContainsKey(key))
        {
            throw ShopManagerExceptionCollection.ContainsKeyException(nameof(key));
        }
    }

    private void EnsureCollectionContainsValue(Product product)
    {
        if (_productsByName[product.Name].All(p => p.Name != product.Name))
        {
            throw ShopManagerExceptionCollection.ContainsValueException(nameof(product));
        }
    }
}