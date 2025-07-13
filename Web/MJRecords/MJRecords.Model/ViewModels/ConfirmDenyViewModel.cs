using MJRecords.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MJRecords.Model
{
    public class ConfirmDenyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Denial reason is required.")]
        [Display(Name = "Denial Reason")]
        [StringLength(500)]
        public string? DenialReason { get; set; }

        public string PurchaseOrderId { get; set; } = string.Empty;

        // Optional display fields
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public byte[]? RecordVersion { get; set; }

        [NotMapped]
        public string? RecordVersionBase64 { get; set; }
    }
}

