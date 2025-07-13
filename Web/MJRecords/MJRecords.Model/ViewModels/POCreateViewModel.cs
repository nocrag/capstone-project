using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class POCreateViewModel
    {

        public string PurchaseOrderId { get; set; }
        public ItemCreateDto NewItem { get; set; } = new();
        public string? EmployeeId { get; set; }
        public string? EmployeeFullName { get; set; }
        public string? EmployeeSupervisor { get; set; }
        public string? EmployeeDepartment { get; set; }

        public decimal Subtotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal GrandTotal { get; set; } = 0;

        public List<PurchaseOrderItem>? ExistingItems { get; set; }

        public POCreateDto Form { get; set; }
    }

}
