using Shops.Models;

namespace Shops.Services;

public interface IShopManager
{
    Customer RegisterCustomer(string name, decimal moneyAmount);

    Shop RegisterShop(string name, string address, decimal markup);
    Shop AddProduct(Shop shop, Product product);

    Product FindProductById(Guid id);
    Shop FindShop(Shop shop);
    Shop FindShopWithCheapestProducts(params Product[] products);

    Shop DeliveryProducts(Shop shop, params Product[] products);
    Shop ChangeProductPrice(Shop shop, Product product, decimal coast);
    void PurchaseProducts(Customer customer, Shop shop, params Product[] products);
}