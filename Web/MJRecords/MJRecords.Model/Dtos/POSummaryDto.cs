using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class POSummaryDto
    {
        public string PurchaseOrderId { get; set; }
        public DateTime DateCreated { get; set; }

        public string Employee { get; set; }
        public PurchaseOrderStatusEnum Status { get; set; } 

        public decimal Subtotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal GrandTotal { get; set; }

        public string EmployeeName { get; set; }

    }

}
