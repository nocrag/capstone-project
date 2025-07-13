using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class POItemEditViewModel
    {
        public int ItemId { get; set; }
        public string? PurchaseOrderId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        public string Justification { get; set; }
        public string PurchaseLocation { get; set; }
        public PurchaseOrderItemStatusEnum Status { get; set; }

        public byte[] RecordVersion { get; set; } = Array.Empty<byte>();


    }
}
