using MJRecords.Model;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MJRecords.Model
{
    public class PurchaseOrder : BaseEntity
    {
        [Key]
        [Display(Name = "PO Number")]
        public string PurchaseOrderId { get; set; }

        [StringLength(8)]
        [Display(Name = "Employee ID")]
        public string? EmployeeId { get; set; }

        [Required]
        [Display(Name = "Order Status")]
        public PurchaseOrderStatusEnum Status { get; set; }

        
        [Display(Name = "Date Created")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime DateCreated { get; set; }

        public byte[] RecordVersion { get; set; }

    }
}
