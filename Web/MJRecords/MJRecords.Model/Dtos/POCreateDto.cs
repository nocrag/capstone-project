using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class POCreateDto
    {
        public string PurchaseOrderId { get; set; }
        public string? EmployeeId { get; set; }
        public ItemCreateDto Item { get; set; }
    }
}
