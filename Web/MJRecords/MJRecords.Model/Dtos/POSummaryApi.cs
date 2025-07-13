using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class POSummaryApiDto
    {
        public string PurchaseOrderId { get; set; } = string.Empty;

        public PurchaseOrderStatusEnum Status { get; set; }

        public int TotalItems { get; set; }

        public decimal GrandTotal { get; set; }

        public string SupervisorName { get; set; } = "No supervisor";
    }

}
