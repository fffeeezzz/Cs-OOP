using Shops.Models;
using Shops.Services;
using Shops.Tools;
using Xunit;

namespace Shops.Test;

public class ShopManagerTest
{
    private readonly IShopManager _shopManager = new ShopManager();

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void RegisterShopWithInvalidName_ThrowException(string name)
    {
        Assert.Throws<ShopManagerException>(() => _shopManager.RegisterShop(name, "address", 0));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CreateProductWithInvalidName_ThrowException(string name)
    {
        Shop shop = _shopManager.RegisterShop("Пятёрочка", "Америка", 1);

        Assert.Throws<ShopManagerException>(() => _shopManager.AddProduct(shop, new Product(name, 0)));
    }

    [Fact]
    public void AddProductToRegisteredShop_ShopContainsProduct()
    {
        Shop shop = _shopManager.RegisterShop("Пятёрочка", "Америка", 1);
        var product = new Product("Кока Кола", 30);

        _shopManager.AddProduct(shop, product);

        Product savedProduct = _shopManager.FindProductById(product.Id);
        Assert.Equal(product.Name, savedProduct.Name);
    }

    [Fact]
    public void RegisterShops_FindShopById()
    {
        Shop shop = _shopManager.RegisterShop("Пятерочка", "Турция", 1);

        Shop savedShop = _shopManager.FindShop(shop);

        Assert.Equal(shop, savedShop);
    }

    [Fact]
    public void CustomerBuyProducts_CustomerHasEnoughMoneyAndAllProductsAreExist()
    {
        Customer customer = _shopManager.RegisterCustomer("Босс", 100000);
        Shop shop = _shopManager.RegisterShop("Пятерочка", "Атлантида", 10);
        var product1 = new Product("Кока кола", 30);
        var product2 = new Product("Кока кола", 30);
        var product3 = new Product("Баунти", 25);
        var product4 = new Product("Сникерс", 20);
        var product5 = new Product("Что то вкусное и дорогое", 1000);
        _shopManager.DeliveryProducts(shop, product1, product2, product3, product4, product5);

        _shopManager.PurchaseProducts(customer, shop, product1, product2, product3, product4, product5);
        int customersCountOfProducts = customer.CountOfProducts();
        Product customersProduct = customer.FindProductById(product3.Id);
        int shopsCountOfProducts = shop.CountOfProducts();

        Assert.True(customer.MoneyAmount < 100000);
        Assert.Equal(100000 - customer.MoneyAmount, shop.Income);
        Assert.Equal(5, customersCountOfProducts);
        Assert.Equal(product3.Name, customersProduct.Name);
        Assert.Equal(0, shopsCountOfProducts);
    }

    [Fact]
    public void ChangeProductPrice_ProductHasNewPrice()
    {
        Shop shop = _shopManager.RegisterShop("Пятерочка", "Зимбабва", 2);
        var product = new Product("Вода", 5);
        _shopManager.AddProduct(shop, product);

        _shopManager.ChangeProductPrice(shop, product, 10);

        decimal newPrice = _shopManager.FindProductById(product.Id).PrimeCoast;
        Assert.Equal(10, newPrice);
    }

    [Fact]
    public void ShopWithCheapestProducts_ShopIsFound()
    {
        var product = new Product("Ананас", 10);
        Shop shop1 = _shopManager.RegisterShop("Пятерочка", "Луна", 12);
        _shopManager.AddProduct(shop1, product);
        Shop shop2 = _shopManager.RegisterShop("Пятерочка", "Миллер", 10);
        _shopManager.AddProduct(shop2, product);
        Shop shop3 = _shopManager.RegisterShop("Пятерочка", "Кеплер", 15);
        _shopManager.AddProduct(shop3, product);

        Shop shop = _shopManager.FindShopWithCheapestProducts(product);

        Assert.Equal(shop2.Id, shop.Id);
    }
}