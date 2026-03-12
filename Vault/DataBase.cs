using System.Collections.Generic;
using Microsoft.VisualBasic;
using Vault.Entities;

namespace Vault
{
    public static class DataBase 
    {
        public static List<User> Users { get; set; } = new List<User>();
        public static List<Product> Products { get; set; } = new List<Product>();
        public static List<PurchaseBatch> Purchases { get; set; } = new List<PurchaseBatch>();
        public static List<SalesTransaction> Sales { get; set; } = new List<SalesTransaction>();

        // Kasa ve Borç değişkenlerimiz
        public static string CurrentUserName { get; set; }
        public static decimal CashInRegister { get; set; } = 50000m; // Başlangıç parası
        public static decimal TotalDebt { get; set; } = 0m;

        public static void ProductList()
        {
            Products.Add(new Product { ProductId = 1, StockCode = "TZG099", Name = "Ankara Makarna", Brand = "Makarnacim A.S.", BaseSalePrice = 50 });
            Products.Add(new Product { ProductId = 2, StockCode = "SUT101", Name = "Tam Yagli Sut", Brand = "Pinar A.S.", BaseSalePrice = 35 });
            Products.Add(new Product { ProductId = 3, StockCode = "EKM202", Name = "Beyaz Ekmek", Brand = "Uno Gida", BaseSalePrice = 20 });
            Products.Add(new Product { ProductId = 4, StockCode = "YGT303", Name = "Suzme Yogurt", Brand = "Sek A.S.", BaseSalePrice = 65 });
            Products.Add(new Product { ProductId = 5, StockCode = "PEY404", Name = "Kasar Peynir", Brand = "Eker A.S.", BaseSalePrice = 120 });
            Products.Add(new Product { ProductId = 6, StockCode = "ZYT505", Name = "Riviera Zeytinyagi", Brand = "Komili A.S.", BaseSalePrice = 180 });
            Products.Add(new Product { ProductId = 7, StockCode = "CYK606", Name = "Filtre Kahve", Brand = "Nescafe A.S.", BaseSalePrice = 95 });
            Products.Add(new Product { ProductId = 8, StockCode = "SKR707", Name = "Kristal Seker", Brand = "Torku A.S.", BaseSalePrice = 45 });
            Products.Add(new Product { ProductId = 9, StockCode = "MRB808", Name = "Cilek Recel", Brand = "Ulker A.S.", BaseSalePrice = 75 });
            Products.Add(new Product { ProductId = 10, StockCode = "CBK909", Name = "Sutlu Cikolata", Brand = "Nestle A.S.", BaseSalePrice = 55 });
        } 
        public static void PurchaseBatchList()
        {
            // Ankara Makarna (ProductId = 1)
            Purchases.Add(new PurchaseBatch { BatchId = 1,  ProductId = 1,  QuantityBought = 100, QuantityRemaining = 100, PurchasePrice = 32, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2026/01/01 10:00:00") });
            Purchases.Add(new PurchaseBatch { BatchId = 2,  ProductId = 1,  QuantityBought = 150, QuantityRemaining = 150, PurchasePrice = 30, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2026/06/01 10:00:00") });

            // Tam Yagli Sut (ProductId = 2)
            Purchases.Add(new PurchaseBatch { BatchId = 3,  ProductId = 2,  QuantityBought = 200, QuantityRemaining = 200, PurchasePrice = 22, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2025/12/01 10:00:00") });
            Purchases.Add(new PurchaseBatch { BatchId = 4,  ProductId = 2,  QuantityBought = 180, QuantityRemaining = 180, PurchasePrice = 21, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2025/11/01 10:00:00") });

            // Beyaz Ekmek (ProductId = 3)
            Purchases.Add(new PurchaseBatch { BatchId = 5,  ProductId = 3,  QuantityBought = 300, QuantityRemaining = 300, PurchasePrice = 12, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2025/10/01 10:00:00") });
            Purchases.Add(new PurchaseBatch { BatchId = 6,  ProductId = 3,  QuantityBought = 250, QuantityRemaining = 250, PurchasePrice = 11, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2025/11/01 10:00:00") });

            // Suzme Yogurt (ProductId = 4)
            Purchases.Add(new PurchaseBatch { BatchId = 7,  ProductId = 4,  QuantityBought = 120, QuantityRemaining = 120, PurchasePrice = 42, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2025/12/01 10:00:00") });
            Purchases.Add(new PurchaseBatch { BatchId = 8,  ProductId = 4,  QuantityBought = 100, QuantityRemaining = 100, PurchasePrice = 40, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2026/01/01 10:00:00") });

            // Kasar Peynir (ProductId = 5)
            Purchases.Add(new PurchaseBatch { BatchId = 9,  ProductId = 5,  QuantityBought = 80,  QuantityRemaining = 80,  PurchasePrice = 80, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2026/03/01 10:00:00") });
            Purchases.Add(new PurchaseBatch { BatchId = 10, ProductId = 5,  QuantityBought = 90,  QuantityRemaining = 90,  PurchasePrice = 78, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2026/06/01 10:00:00") });

            // Riviera Zeytinyagi (ProductId = 6)
            Purchases.Add(new PurchaseBatch { BatchId = 11, ProductId = 6,  QuantityBought = 60,  QuantityRemaining = 60,  PurchasePrice = 120, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2027/01/01 10:00:00") });
            Purchases.Add(new PurchaseBatch { BatchId = 12, ProductId = 6,  QuantityBought = 70,  QuantityRemaining = 70,  PurchasePrice = 115, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2027/06/01 10:00:00") });

            // Filtre Kahve (ProductId = 7)
            Purchases.Add(new PurchaseBatch { BatchId = 13, ProductId = 7,  QuantityBought = 110, QuantityRemaining = 110, PurchasePrice = 62, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2027/01/01 10:00:00") });
            Purchases.Add(new PurchaseBatch { BatchId = 14, ProductId = 7,  QuantityBought = 130, QuantityRemaining = 130, PurchasePrice = 60, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2027/06/01 10:00:00") });

            // Kristal Seker (ProductId = 8)
            Purchases.Add(new PurchaseBatch { BatchId = 15, ProductId = 8,  QuantityBought = 200, QuantityRemaining = 200, PurchasePrice = 28, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2028/01/01 10:00:00") });
            Purchases.Add(new PurchaseBatch { BatchId = 16, ProductId = 8,  QuantityBought = 220, QuantityRemaining = 220, PurchasePrice = 27, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2028/06/01 10:00:00") });

            // Cilek Recel (ProductId = 9)
            Purchases.Add(new PurchaseBatch { BatchId = 17, ProductId = 9,  QuantityBought = 90,  QuantityRemaining = 90,  PurchasePrice = 48, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2027/01/01 10:00:00") });
            Purchases.Add(new PurchaseBatch { BatchId = 18, ProductId = 9,  QuantityBought = 85,  QuantityRemaining = 85,  PurchasePrice = 46, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2027/06/01 10:00:00") });

            // Sutlu Cikolata (ProductId = 10)
            Purchases.Add(new PurchaseBatch { BatchId = 19, ProductId = 10, QuantityBought = 150, QuantityRemaining = 150, PurchasePrice = 35, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2026/06/01 10:00:00") });
            Purchases.Add(new PurchaseBatch { BatchId = 20, ProductId = 10, QuantityBought = 160, QuantityRemaining = 160, PurchasePrice = 34, PurchaseDate = DateTime.Now, ExpirationDate = Convert.ToDateTime("2026/12/01 10:00:00") });
          }

        public static void MockSalesList()
        {
            // 15 Ocak 2026'da 20 paket Makarna satılmış olsun
            Sales.Add(new SalesTransaction { 
                SaleId = 1, ProductId = 1, QuantitySold = 20, SalePrice = 50m, TotalCost = 640m, SaleDate = new DateTime(2026, 1, 15) 
            });

            // 10 Şubat 2026'da 30 şişe Süt satılmış olsun
            Sales.Add(new SalesTransaction { 
                SaleId = 2, ProductId = 2, QuantitySold = 30, SalePrice = 35m, TotalCost = 660m, SaleDate = new DateTime(2026, 2, 10) 
            });

            // 5 Mart 2026'da 15 Ekmek satılmış olsun
            Sales.Add(new SalesTransaction { 
                SaleId = 3, ProductId = 3, QuantitySold = 15, SalePrice = 20m, TotalCost = 180m, SaleDate = new DateTime(2026, 3, 5) 
            });
        }
        public static void InitializeData()
        {
            if (Users.Count == 0)
            {
                Users.Add(new User { UserId = 1, Username = "zey", FullName = "Zeynepnur Gold", Phone = "555", Title = "Admin" });
                Users.Add(new User { UserId = 2, Username = "gold", FullName = "Tahsin Gold", Phone = "555", Title = "Manager" });
                Users.Add(new User { UserId = 3, Username = "Misanur", FullName = "Misha Erbek", Phone = "555", Title = "Staff" });
                Users.Add(new User { UserId = 4, Username = "pakuga", FullName = "Fatma Erbek", Phone = "555", Title = "Admin" });
                Users.Add(new User { UserId = 5, Username = "urzul", FullName = "Emre Erbek", Phone = "555", Title = "Admin" });
            }

            // 2. ÜRÜN, ALIM VE SATIŞ VERİLERİNİ YÜKLE (Bilanço ve Raporlar için)
            if (Products.Count == 0) ProductList();
            if (Purchases.Count == 0) PurchaseBatchList();
            if (Sales.Count == 0) MockSalesList(); // Bir önceki adımda eklediğimiz geçmiş satışlar

            // 3. BAŞLANGIÇ KASASI VE BORCU (Gerçekçi Bilanço İçin)
            // Sadece program ilk açıldığında varsayılan değerdeyse ezsin diye ufak bir kontrol ekledik.
            if (CashInRegister == 50000m && TotalDebt == 0m) 
            {
                CashInRegister = 25000m; // Kasada 25 bin TL nakit var
                TotalDebt = 15000m;      // Toptancıya 15 bin TL borç var
            }
        }
    }
}