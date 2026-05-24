using System;
using System.Collections.Generic;

namespace MPSPDotNetTraining.MiniPosSystem.Models.Responses
{
    public class SaleResponse
    {
        public int SaleId { get; set; }

        public DateTime SaleDate { get; set; }

        public decimal TotalAmount { get; set; }

        public List<SaleItemResponse> Items { get; set; } = new();
    }
}