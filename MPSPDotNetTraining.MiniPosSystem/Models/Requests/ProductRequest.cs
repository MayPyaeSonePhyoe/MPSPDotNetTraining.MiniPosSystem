namespace MPSPDotNetTraining.MiniPosSystem.Models.Requests
{
    public class ProductRequest
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int StockQty { get; set; }

        public int CategoryId { get; set; }
    }
}