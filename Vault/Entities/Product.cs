namespace Vault.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string StockCode { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal BaseSalePrice { get; set; } 
    }
}