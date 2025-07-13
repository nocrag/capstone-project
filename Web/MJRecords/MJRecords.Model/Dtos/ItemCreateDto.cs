using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class ItemCreateDto
    {
        public string PurchaseOrderId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Justification { get; set; }
        public string PurchaseLocation { get; set; }
        public PurchaseOrderItemStatusEnum Status { get; set; }

        public byte[] RecordVersion { get; set; } = Array.Empty<byte>();
    }
}

