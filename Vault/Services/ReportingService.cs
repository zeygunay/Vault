using System;
using System.Linq;
using Vault.Entities;

namespace Vault.Services
{
    public class ReportingService
    {
        public void ReportingMenu()
        {
            Console.Clear();
            Console.WriteLine("--- FINANCIAL REPORTS & BALANCE SHEET ---");
            Console.WriteLine("1. Date Range Profit/Loss Report (Tarih Aralikli Kar/Zarar Raporu)");
            Console.WriteLine("2. Current Stock Inventory Table (Stok Envanter Tablosu)");
            Console.WriteLine("3. General Balance Sheet (Genel Bilanco)");
            Console.Write("Select an action: ");
            string action = Console.ReadLine();

            if (action == "1") DateRangeReport();
            else if (action == "2") StockInventoryReport();
            else if (action == "3") GeneralBalanceSheet();

            Console.WriteLine("\nPress Enter to return to Main Menu...");
            Console.ReadLine();
        }

        private void DateRangeReport()
        {
            Console.WriteLine("\n-- Date Range Profit & Loss Report --");
            
            // 1. Yılı Sor
            Console.Write("Enter Target Year (e.g., 2026) or press Enter for current year: ");
            string yearStr = Console.ReadLine();
            int year = string.IsNullOrWhiteSpace(yearStr) ? DateTime.Now.Year : int.Parse(yearStr);

            // 2. Ay Aralığını Sor
            Console.Write("Enter Start Month (1-12) or 0 for the whole year: ");
            int.TryParse(Console.ReadLine(), out int startMonth);

            int endMonth = 12;
            if (startMonth > 0)
            {
                Console.Write("Enter End Month (1-12): ");
                int.TryParse(Console.ReadLine(), out endMonth);
            }
            else
            {
                startMonth = 1; // Tüm yıl seçildiyse Ocak'tan başla
            }

            // Tarih sınırlarını oluştur
            DateTime startDate = new DateTime(year, startMonth, 1);
            int lastDayOfEndMonth = DateTime.DaysInMonth(year, endMonth);
            DateTime endDate = new DateTime(year, endMonth, lastDayOfEndMonth, 23, 59, 59);

            // LINQ ile Tarih Filtreleme
            var filteredSales = DataBase.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToList();

            if (!filteredSales.Any())
            {
                Console.WriteLine($"\nNo sales found between {startDate:dd.MM.yyyy} and {endDate:dd.MM.yyyy}.");
                return;
            }

            // Hesaplamalar
            decimal totalRevenue = filteredSales.Sum(s => s.QuantitySold * s.SalePrice);
            decimal totalCost = filteredSales.Sum(s => s.TotalCost);
            decimal netProfit = totalRevenue - totalCost;
            int totalItemsSold = filteredSales.Sum(s => s.QuantitySold);

            Console.WriteLine($"\n=======================================================");
            Console.WriteLine($"  REPORT: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
            Console.WriteLine($"=======================================================");
            Console.WriteLine($"Total Items Sold (Satilan Urun Adedi) : {totalItemsSold}");
            Console.WriteLine($"Total Revenue (Toplam Ciro / Kasa)    : {totalRevenue} TL");
            Console.WriteLine($"Total Cost (Urunlerin Maliyeti)       : {totalCost} TL");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine($"NET PROFIT/LOSS (KAR/ZARAR)           : {netProfit} TL");
            Console.WriteLine($"=======================================================\n");

            // Satışların detaylı tablosu (İmzalı Versiyon)
            Console.WriteLine("Sales Details (Satis Detaylari):");
            Console.WriteLine(string.Format("{0,-5} | {1,-20} | {2,-5} | {3,-12} | {4,-12} | {5,-25}", "ID", "Product", "Qty", "Revenue", "Profit", "Performed By"));
            Console.WriteLine(new string('-', 90)); // Çizgiyi tabloya göre uzattık

            foreach (var sale in filteredSales)
            {
                var pName = DataBase.Products.First(p => p.ProductId == sale.ProductId).Name;
                decimal saleProfit = (sale.QuantitySold * sale.SalePrice) - sale.TotalCost;
                
                // Eğer eski mock verilerde isim yoksa "Unknown" yazsın, yenilerde imza görünsün
                string performedBy = string.IsNullOrWhiteSpace(sale.PerformedBy) ? "System/Unknown" : sale.PerformedBy;
                
                Console.WriteLine(string.Format("{0,-5} | {1,-20} | {2,-5} | {3,-12} | {4,-12} | {5,-25}", 
                    sale.SaleId, pName, sale.QuantitySold, (sale.QuantitySold * sale.SalePrice) + " TL", saleProfit + " TL", performedBy));
            }
        }

        private void StockInventoryReport()
        {
            Console.WriteLine("\n-- Current Stock Inventory Table --");
            Console.WriteLine(string.Format("{0,-5} | {1,-20} | {2,-15} | {3,-15}", "ID", "Product Name", "Total Stock", "Inventory Value"));
            Console.WriteLine(new string('-', 65));

            decimal totalInventoryValue = 0;

            foreach (var p in DataBase.Products)
            {
                int currentStock = DataBase.Purchases.Where(b => b.ProductId == p.ProductId).Sum(b => b.QuantityRemaining);
                decimal inventoryValue = DataBase.Purchases.Where(b => b.ProductId == p.ProductId).Sum(b => b.QuantityRemaining * b.PurchasePrice);
                
                totalInventoryValue += inventoryValue;

                Console.WriteLine(string.Format("{0,-5} | {1,-20} | {2,-15} | {3,-15}", 
                    p.ProductId, p.Name, currentStock, inventoryValue + " TL"));
            }

            Console.WriteLine(new string('-', 65));
            Console.WriteLine($"Total Value of Goods in Warehouse (Depodaki Mallarin Toplam Degeri): {totalInventoryValue} TL");
        }

        private void GeneralBalanceSheet()
        {
            Console.WriteLine("\n=========================================");
            Console.WriteLine("        GENERAL BALANCE SHEET  ");
            Console.WriteLine("=========================================");
            
            decimal warehouseValue = DataBase.Purchases.Sum(b => b.QuantityRemaining * b.PurchasePrice);
            
            Console.WriteLine("--- ASSETS (VARLIKLAR) ---");
            Console.WriteLine($"Cash in Register (Kasadaki Nakit) : {DataBase.CashInRegister} TL");
            Console.WriteLine($"Warehouse Value (Depodaki Mal)    : {warehouseValue} TL");
            Console.WriteLine($"TOTAL ASSETS (Toplam Varlik)      : {DataBase.CashInRegister + warehouseValue} TL\n");

            Console.WriteLine("--- LIABILITIES (YUKUMLULUKLER) ---");
            Console.WriteLine($"Total Debt (Toptanciya Borc)      : {DataBase.TotalDebt} TL");
            
            Console.WriteLine("\n=========================================");
            Console.WriteLine($"COMPANY NET WORTH (SIRKET NET DEGERI): {(DataBase.CashInRegister + warehouseValue) - DataBase.TotalDebt} TL");
            Console.WriteLine("=========================================");
        }
    }
}