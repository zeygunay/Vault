using System;
using System.Linq;
using Vault.Entities;

namespace Vault.Services
{
    public class UserService
    {
        // Sisteme giren kullanıcıyı burada tutuyoruz
        public User LoggedInUser { get; private set; }

        public void Login()
        {
            while (LoggedInUser == null)
            {
                Console.Clear();
                Console.WriteLine("--- SYSTEM LOGIN ---");
                Console.Write("Enter Username: ");
                string inputUsername = Console.ReadLine();

                // DataBase içindeki Users listesinde bu kullanıcı adını arıyoruz
                LoggedInUser = DataBase.Users.FirstOrDefault(u => u.Username.ToLower() == inputUsername?.ToLower());

                if (LoggedInUser == null)
                {
                    Console.WriteLine("User not found! Press Enter to try again.");
                    Console.ReadLine();
                }
                DataBase.CurrentUserName = $"{LoggedInUser.FullName} ({LoggedInUser.Title})";
            }
        }

        public void ManageUsers()
        {
            Console.Clear();
            Console.WriteLine("--- USER MANAGEMENT ---");

            if (LoggedInUser.Title != "Admin")
            {
                Console.WriteLine("Access Denied! Only 'Admin' can manage users.");
                Console.WriteLine("Press Enter to return...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("1. Add New User");
            Console.WriteLine("2. Delete User");
            Console.WriteLine("3. List Users");
            Console.Write("Select an action: ");
            string action = Console.ReadLine();

            if (action == "1")
            {
                AddUser();
            }
            else if (action == "2")
            {
                DeleteUser();
            }
            else if (action == "3")
            {
                ListUsers();
            }

            Console.WriteLine("\nPress Enter to return to Main Menu...");
            Console.ReadLine();
        }

        private void AddUser()
        {
            Console.WriteLine("\n-- Add New User --");
            User newUser = new User();
            
            // Eğer listede kullanıcı varsa en yüksek ID'yi bulup 1 ekler, yoksa 1 verir
            newUser.UserId = DataBase.Users.Any() ? DataBase.Users.Max(u => u.UserId) + 1 : 1;

            Console.Write("Username: ");
            newUser.Username = Console.ReadLine();
            Console.Write("Full Name: ");
            newUser.FullName = Console.ReadLine();
            Console.Write("Phone: ");
            newUser.Phone = Console.ReadLine();
            Console.Write("Title (Admin/Staff): ");
            newUser.Title = Console.ReadLine();

            DataBase.Users.Add(newUser);
            Console.WriteLine($"User '{newUser.Username}' added successfully!");
        }

        private void DeleteUser()
        {
            Console.WriteLine("\n-- Delete User --");
            Console.Write("Enter the ID of the user to delete: ");
            if (int.TryParse(Console.ReadLine(), out int idToDelete))
            {
                if (idToDelete == LoggedInUser.UserId)
                {
                    Console.WriteLine("You cannot delete yourself while logged in!");
                    return;
                }

                var userToRemove = DataBase.Users.FirstOrDefault(u => u.UserId == idToDelete);
                if (userToRemove != null)
                {
                    DataBase.Users.Remove(userToRemove);
                    Console.WriteLine("User deleted successfully.");
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

        private void ListUsers()
        { 
            Console.WriteLine("\n-- User List --");
            foreach (var u in DataBase.Users)
            {
                Console.WriteLine($"ID: {u.UserId} | Username: {u.Username} | Name: {u.FullName} | Title: {u.Title}");
            }
        }
    }
}