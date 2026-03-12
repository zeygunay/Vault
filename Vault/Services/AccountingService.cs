using System;
using System.Linq;
using Vault;
using Vault.Entities;
namespace AccountingSystem.Services;

public class AccountingService
{
    public void AccountingMenu()
    {
        Console.Clear();
        Console.WriteLine("--- SALES & ACCOUNTING MANAGEMENT---");
        Console.WriteLine("1. Sell Product");//satis yap
        Console.WriteLine("2. Pay Debt");//borc ode
        Console.WriteLine("Select an action:");
        string action = Console.ReadLine();
        if (action == "1") SellProduct();
        else if (action == "2") PayDebt();
        Console.WriteLine("\nPress Enter to return to Main Menu..");
        Console.ReadLine();
    }

    private void SellProduct()
    {
        Console.WriteLine("\n-- Sell Product --");

        var availableProducts = DataBase.Products
            .Where(p => DataBase.Purchases.Any(b => b.ProductId == p.ProductId && b.QuantityRemaining > 0))
            .ToList();

        if (!availableProducts.Any())
        {
            Console.WriteLine("No products available in stock to sell!");
            return;
        }

        Console.WriteLine("Available Products In Stock:");
        foreach (var p in availableProducts)
        {
            int totalStock = DataBase.Purchases.Where(b => b.ProductId == p.ProductId).Sum(b => b.QuantityRemaining);
            Console.WriteLine($"ID: '{p.ProductId}' | Name: '{p.Name}' | Total Stock: {totalStock} | Unit Sale Price: {p.BaseSalePrice} TL");
        }

        int prodId; 
        Console.Write("\nEnter Product ID to sell: ");
        if (!int.TryParse(Console.ReadLine(), out prodId)) 
        {
            Console.WriteLine("Invalid input. Cancelling...");
            return;
        }

        var product = availableProducts.FirstOrDefault(p => p.ProductId == prodId);
        if (product == null)
        {
            Console.WriteLine("Invalid Product ID or out of stock.");
            return;
        }

        int totalAvailableStock = DataBase.Purchases.Where(b => b.ProductId == prodId).Sum(b => b.QuantityRemaining);

        int qtyToSell;
        Console.Write($"Quantity to sell (Max {totalAvailableStock}): ");
        if (!int.TryParse(Console.ReadLine(), out qtyToSell) || qtyToSell <= 0) 
        {
            Console.WriteLine("Invalid quantity. Cancelling...");
            return;
        }

        if (qtyToSell > totalAvailableStock)
        {
            Console.WriteLine("Error: Not enough stock!");
            return;
        }

        var batchesToUse = DataBase.Purchases
            .Where(b => b.ProductId == prodId && b.QuantityRemaining > 0)
            .OrderBy(b => b.ExpirationDate) 
            .ToList();

        int remainingQtyToFulfill = qtyToSell;
        decimal totalCostForThisSale = 0; 

        foreach (var batch in batchesToUse)
        {
            if (remainingQtyToFulfill == 0) break; 

            if (batch.QuantityRemaining >= remainingQtyToFulfill)
            {
                batch.QuantityRemaining -= remainingQtyToFulfill;
                totalCostForThisSale += remainingQtyToFulfill * batch.PurchasePrice;
                remainingQtyToFulfill = 0;
            }
            else
            {
                remainingQtyToFulfill -= batch.QuantityRemaining;
                totalCostForThisSale += batch.QuantityRemaining * batch.PurchasePrice;
                batch.QuantityRemaining = 0; 
            }
        }

        // Satış İşlemini Kaydetme
        decimal totalSalePrice = qtyToSell * product.BaseSalePrice;
        
        SalesTransaction newSale = new SalesTransaction
        {
            SaleId = DataBase.Sales.Any() ? DataBase.Sales.Max(s => s.SaleId) + 1 : 1,
            ProductId = product.ProductId,
            QuantitySold = qtyToSell,
            SalePrice = product.BaseSalePrice,
            TotalCost = totalCostForThisSale,
            SaleDate = DateTime.Now,
            PerformedBy = DataBase.CurrentUserName 
        };

        DataBase.Sales.Add(newSale);

        DataBase.CashInRegister += totalSalePrice;

        Console.WriteLine($"\nSUCCESS: {qtyToSell}x '{product.Name}' sold by {newSale.PerformedBy}!");        
        Console.WriteLine($"Total Revenue : {totalSalePrice} TL");
        Console.WriteLine($"Total Cost : {totalCostForThisSale} TL");
        Console.WriteLine($"Profit from this sale : {totalSalePrice - totalCostForThisSale} TL");
        decimal allTimeRevenue = DataBase.Sales.Sum(s => s.QuantitySold * s.SalePrice);
        decimal allTimeCost = DataBase.Sales.Sum(s => s.TotalCost);
        decimal allTimeProfit = allTimeRevenue - allTimeCost;

        Console.WriteLine("\n--- CURRENT FINANCIAL SNAPSHOT ---");
        Console.WriteLine($"Total Cash in Register : {DataBase.CashInRegister} TL");
        Console.WriteLine($"Total Debt : {DataBase.TotalDebt} TL");
        Console.WriteLine($"All-Time Total Sales Revenue : {allTimeRevenue} TL");
        Console.WriteLine($"All-Time Net Profit : {allTimeProfit} TL");
    }
        private void PayDebt()
        {
            Console.WriteLine("\n-- Pay Debt --");
            Console.WriteLine($"Current Debt : {DataBase.TotalDebt} TL");
            Console.WriteLine($"Current Cash in Register : {DataBase.CashInRegister} TL");

            if (DataBase.TotalDebt <= 0)
            {
                Console.WriteLine("You have no debt to pay. Great job!");
                return;
            }

            Console.Write("\nEnter amount to pay: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amountToPay))
            {
                if (amountToPay <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return;
                }

                if (amountToPay > DataBase.CashInRegister)
                {
                    Console.WriteLine("Error: You don't have enough cash in the register to make this payment!");
                    return;
                }

                if (amountToPay > DataBase.TotalDebt)
                {
                    Console.WriteLine("Note: You entered an amount greater than your debt. Paying off the exact debt amount.");
                    amountToPay = DataBase.TotalDebt;
                }

                // Muhasebe Güncellemesi: Kasadan düş, Borçtan düş
                DataBase.CashInRegister -= amountToPay;
                DataBase.TotalDebt -= amountToPay;

                Console.WriteLine($"\nSUCCESS: {amountToPay} TL paid towards your debt.");
                Console.WriteLine($"Remaining Debt: {DataBase.TotalDebt} TL");
                Console.WriteLine($"Remaining Cash: {DataBase.CashInRegister} TL");
            }
        }
    }