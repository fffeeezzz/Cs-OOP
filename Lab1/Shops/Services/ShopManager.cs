using Shops.Models;
using Shops.Tools;

namespace Shops.Services;

public class ShopManager : IShopManager
{
    private readonly List<Customer> _customers;
    private readonly List<Shop> _shops;
    private int _customerId = 1;
    private int _shopId = 1;

    public ShopManager()
    {
        _shops = new List<Shop>();
        _customers = new List<Customer>();
    }

    public Customer RegisterCustomer(string name, decimal moneyAmount)
    {
        var customer = new Customer(name, moneyAmount, _customerId++);
        if (IsCustomerExist(customer))
        {
            throw ShopManagerExceptionCollection.NotUniqueException(nameof(customer));
        }

        _customers.Add(customer);

        return customer;
    }

    public Shop RegisterShop(string name, string address, decimal markup)
    {
        var shop = new Shop(name, address, markup, _shopId++);
        if (IsShopExist(shop))
        {
            throw ShopManagerExceptionCollection.NotUniqueException(nameof(shop));
        }

        _shops.Add(shop);

        return shop;
    }

    public Shop AddProduct(Shop shop, Product product)
    {
        if (!IsShopExist(shop))
        {
            throw ShopManagerExceptionCollection.ContainsValueException(nameof(shop));
        }

        return shop.AddProduct(product);
    }

    public Product FindProductById(Guid id)
    {
        return _shops.Select(shop => shop.FindProductById(id)).FirstOrDefault();
    }

    public Shop FindShop(Shop shop)
    {
        return _shops.FirstOrDefault(s => Equals(shop, s));
    }

    public Shop FindShopWithCheapestProducts(params Product[] products)
    {
        Shop shopWithCheapestProducts = null;
        decimal productsSum = decimal.MaxValue;
        foreach (Shop shop in _shops)
        {
            if (!shop.ContainsAllProducts(products)) continue;

            decimal sum = shop.GetProductsSum(products);
            if (sum >= productsSum) continue;

            productsSum = sum;
            shopWithCheapestProducts = shop;
        }

        return shopWithCheapestProducts;
    }

    public Shop DeliveryProducts(Shop shop, params Product[] products)
    {
        if (!IsShopExist(shop))
        {
            throw ShopManagerExceptionCollection.ContainsValueException(nameof(shop));
        }

        return shop.AddProducts(products);
    }

    public Shop ChangeProductPrice(Shop shop, Product product, decimal coast)
    {
        Product savedProduct = shop.FindProductById(product.Id);
        if (savedProduct is null)
        {
            throw ShopManagerExceptionCollection.IsNullException(nameof(savedProduct));
        }

        savedProduct.ChangePrimeCoast(coast);

        return shop;
    }

    public void PurchaseProducts(Customer customer, Shop shop, params Product[] products)
    {
        EnsureCustomerAndShopAreValid(customer, shop);

        shop.PurchaseProducts(customer, products);
    }

    private bool IsShopExist(Shop shop) => _shops.Any(s => Equals(shop, s));
    private bool IsCustomerExist(Customer customer) => _customers.Any(c => c.Id == customer.Id);

    private void EnsureCustomerAndShopAreValid(Customer customer, Shop shop)
    {
        if (customer is null)
        {
            throw ShopManagerExceptionCollection.IsNullException(nameof(customer));
        }

        if (!IsCustomerExist(customer))
        {
            throw ShopManagerExceptionCollection.ContainsValueException(nameof(customer));
        }

        if (shop is null)
        {
            throw ShopManagerExceptionCollection.IsNullException(nameof(shop));
        }

        if (!IsShopExist(shop))
        {
            throw ShopManagerExceptionCollection.ContainsValueException(nameof(shop));
        }
    }
}