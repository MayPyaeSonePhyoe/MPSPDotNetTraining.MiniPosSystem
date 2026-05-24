namespace MPSPDotNetTraining.MiniPosSystem.Models.Entities
{
    public class Sale
    {
        public int Id { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public List<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}