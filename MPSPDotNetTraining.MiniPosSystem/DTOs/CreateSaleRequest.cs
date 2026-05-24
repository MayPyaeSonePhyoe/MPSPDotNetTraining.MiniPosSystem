using System.Collections.Generic;

namespace MPSPDotNetTraining.MiniPosSystem.DTOs
{
    public class CreateSaleRequest
    {
        public List<SaleItemRequest> Items { get; set; } = new();
    }
}