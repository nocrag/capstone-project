using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class PODetailsViewModel
    {
        public string PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public List<PurchaseOrderItem> Items { get; set; }

        public ItemCreateDto NewItem { get; set; } = new();

        public string EmployeeId;
        public string EmployeeFullName { get; set; }
        public string? Department { get; set; }
        public string? Supervisor { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal GrandTotal { get; set; }

        public byte[] RecordVersion { get; set; } = Array.Empty<byte>();
    }
}
