using System;

namespace Vault.Entities
{
    public class PurchaseBatch
    {
        public int BatchId { get; set; }
        public int ProductId { get; set; }
        public int QuantityBought { get; set; }//alinan miktar
        public int QuantityRemaining { get; set; } // Satıldıkça buradan düşeceğiz
        public decimal PurchasePrice { get; set; } 
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string PerformedBy { get; set; }
    }
}