namespace Vault.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; } // Örn: "Admin", "Staff"
    }
}