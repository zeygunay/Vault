using System;

namespace Vault.Entities
{
    public class SalesTransaction
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int QuantitySold { get; set; }
        public decimal SalePrice { get; set; }
        public decimal TotalCost { get; set; } // FIFO maliyeti buraya yazılacak
        public DateTime SaleDate { get; set; }
        public string PerformedBy { get; set; }
    }
}