using System;
using System.Collections.Generic;
using AccountingSystem.Services;
using Vault.Services;

namespace Vault
{
    class Program
    {
        static bool isRunning = true;
        
        // Service'imizi başlatıyoruz
        
        static UserService userService = new UserService();
        static InventoryService inventoryService = new InventoryService();
        static AccountingService accountingService = new AccountingService();
        static ReportingService reportingService = new ReportingService();
        // Dinamik menümüz. Metot referanslarını (Action) tutuyor.
        static Dictionary<string, Action> menuActions = new Dictionary<string, Action>
        {
            { "1", () => userService.ManageUsers() },
            { "2", () => inventoryService.InventoryMenu() },
            { "3", () => accountingService.AccountingMenu() },
            { "4", () => reportingService.ReportingMenu() },
            { "5", ExitProgram }
        };

        static void Main(string[] args)
        {
            // Program başlarken sahte admini DataBase'e yüklüyoruz
            DataBase.InitializeData();

            // Kullanıcı girişi ekranı
            userService.Login();

            // Ana Döngü
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine($"  VAULT ACCOUNTING - Welcome, {userService.LoggedInUser.FullName} ({userService.LoggedInUser.Title})");
                Console.WriteLine("========================================");
                Console.WriteLine($"Cash in Register: {DataBase.CashInRegister} TL | Total Debt: {DataBase.TotalDebt} TL\n");
                
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("1. Manage Users (Add/Delete)");
                Console.WriteLine("2. Enter New Product (Purchase / Stock In)");
                Console.WriteLine("3. Sell Product (Stock Out)");
                Console.WriteLine("4. View Financial Summary");
                Console.WriteLine("5. Exit");
                Console.Write("\nPlease select an option (1-5): ");

                string choice = Console.ReadLine();

                if (menuActions.ContainsKey(choice))
                {
                    menuActions[choice].Invoke();
                }
                else
                {
                    Console.WriteLine("Invalid selection. Press Enter to try again.");
                    Console.ReadLine();
                }
            }
        }

        // Şimdilik yer tutucu (Placeholder) metotlar. Bunları 2., 3. ve 4. bölümlerde ayrı Service sınıflarına taşıyacağız.
/*static void BuyProductPlaceholder() 
        { 
            Console.WriteLine("Inventory module coming in Part 2..."); 
            Console.ReadLine(); 
        }
        static void SellProductPlaceholder() 
        { 
            Console.WriteLine("Sales module coming in Part 3..."); 
            Console.ReadLine(); 
        }
        static void ViewSummaryPlaceholder() 
        { 
            Console.WriteLine("Reporting module coming in Part 4..."); 
            Console.ReadLine(); 
        }**/
        
        static void ExitProgram()
        {
            Console.WriteLine("Exiting the program. Goodbye!");
            isRunning = false;
        }
    }
}