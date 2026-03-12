using System;
using System.Linq;
using Vault;
using Vault.Entities;
namespace AccountingSystem.Services;

public class InventoryService
{
    public void InventoryMenu()
    {
        Console.Clear();
        Console.WriteLine("--- INVENTORY & PURCHASE MANAGEMENT ---");
        Console.WriteLine("1. Define New  Product");//katalogoumuza yeni urun olusturuyoruz
        Console.WriteLine("2. Buy Product");//stokgiris mal alim
        Console.WriteLine("3. List Product Catalog");//katalog gor
        Console.WriteLine("Select an action:");
        string action = Console.ReadLine();
        if (action == "1") DefineProduct();
        else if (action == "2") BuyProduct();
        else if (action == "3") ListProduct();
        Console.WriteLine("\nPress Enter to return to Main Menu..");
        Console.ReadLine();
    }

    public void DefineProduct()
    {
        Console.WriteLine("\n-- Define New Product --");
        Product newProduct = new Product();
        
        //otoId atama
        newProduct.ProductId = DataBase.Products.Any()? DataBase.Products.Max(x => x.ProductId) + 1: 1;
        Console.Write(" Product Stock Code: ");
        newProduct.StockCode = Console.ReadLine();
        Console.Write("Product Name: ");
        newProduct.Name = Console.ReadLine();
        Console.Write("Product Brand: ");
        newProduct.Brand = Console.ReadLine();
        
        Console.Write("Base Sale Price: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            newProduct.BaseSalePrice = price;
        }
        DataBase.Products.Add(newProduct);
        Console.Write($"\nSUCCSES: Product'{newProduct.Name}' added to the catalog");
    }
    public void BuyProduct()
    {
        Console.WriteLine("\n-- Buy Product (Stock In) --");
        if (!DataBase.Products.Any())
        {
            Console.WriteLine("No products defined yet! Please define a product first (Option 1).");
            return;
        }

        Console.WriteLine("Available Products:");
        foreach(var p in DataBase.Products)
        {
            Console.WriteLine($"ID: {p.ProductId} | {p.Name} ({p.Brand}) | Current Sale Price: {p.BaseSalePrice} TL");
        }

        Console.Write("\nEnter Product ID to buy: ");
        if (!int.TryParse(Console.ReadLine(), out int prodId)) return;

        var product = DataBase.Products.FirstOrDefault(p => p.ProductId == prodId);
        if (product == null)
        {
            Console.WriteLine("Product not found!");
            return;
        }

        PurchaseBatch batch = new PurchaseBatch();
        batch.BatchId = DataBase.Purchases.Any() ? DataBase.Purchases.Max(b => b.BatchId) + 1 : 1;
        batch.ProductId = product.ProductId;
        batch.PurchaseDate = DateTime.Now;

        Console.Write("Quantity to buy (Kac adet): ");
        batch.QuantityBought = int.Parse(Console.ReadLine());
        batch.QuantityRemaining = batch.QuantityBought;

        Console.Write("Unit Purchase Price (Alis fiyati): ");
        batch.PurchasePrice = decimal.Parse(Console.ReadLine());

        // YENİ EKLENEN ENFLASYON/FİYAT GÜNCELLEME KISMI
        Console.Write($"\nCurrent Sale Price is {product.BaseSalePrice} TL. Enter NEW Sale Price (or press Enter to keep current): ");
        string newSalePriceStr = Console.ReadLine();
        if (decimal.TryParse(newSalePriceStr, out decimal newSalePrice))
        {
            product.BaseSalePrice = newSalePrice; // Eski ürünün satış fiyatı tüm urunler için güncellendi!
            Console.WriteLine($"Sale Price updated to {newSalePrice} TL.");
        }

        Console.Write("\nExpiration Date (dd.mm.yyyy - Örn: 30.12.2026) or press Enter to skip: ");
        string expDateStr = Console.ReadLine();
        
        if (DateTime.TryParseExact(expDateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime expDate))
        {
            batch.ExpirationDate = expDate;
        }
        else
        { 
            batch.ExpirationDate = DateTime.Now.AddYears(10);
            Console.WriteLine($"Info: No date provided or invalid format. Expiration date automatically set to {batch.ExpirationDate:dd.MM.yyyy}.");
        }

        decimal totalCost = batch.QuantityBought * batch.PurchasePrice;
        Console.WriteLine($"\nTotal Cost of this purchase: {totalCost} TL.");
        
        DataBase.TotalDebt += totalCost;
        Console.WriteLine($"Amount ({totalCost} TL) automatically added to Total Debt.");
        batch.PerformedBy = DataBase.CurrentUserName; // İşlemi kim yaptıysa kaydet

        DataBase.Purchases.Add(batch);
        Console.WriteLine($"SUCCESS: Purchase completed and stock added by {batch.PerformedBy}!");
    }

    public void ListProduct()
    {
        Console.WriteLine("\n-- Product Catalog & Active Batches --");
        
        foreach (var p in DataBase.Products)
        {
            // Total Current Stock satırı tamamen kaldırıldı. Sadece ana ürün bilgisi yazdırılıyor.
            Console.WriteLine($"\nID:'{p.ProductId}' | StockCode: '{p.StockCode}' | Name:'{p.Name}' | Brand:'{p.Brand}' | Sale Price:'{p.BaseSalePrice} TL'");

            var activeBatches = DataBase.Purchases
                .Where(b => b.ProductId == p.ProductId && b.QuantityRemaining > 0)
                .OrderBy(b => b.ExpirationDate) // FıFO sırasına göre gösterir
                .ToList();

            if (activeBatches.Any())
            {
                foreach (var batch in activeBatches)
                {
                    Console.WriteLine($"   -> Batch ID: '{batch.BatchId}' | Bought: '{batch.QuantityBought}' | REMAINING: '{batch.QuantityRemaining}' | Exp: '{batch.ExpirationDate:yyyy/MM/dd}' | By: {batch.PerformedBy}");                }
            }
            else
            {
                Console.WriteLine("   -> OUT OF STOCK");
            }
        }
    }
}