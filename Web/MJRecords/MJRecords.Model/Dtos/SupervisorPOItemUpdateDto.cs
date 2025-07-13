using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class SupervisorPOItemUpdateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than zero.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 9999999, ErrorMessage = "Price must be greater than zero.")]
        [Display(Name = "Unit Price")]
        public decimal Price { get; set; }

        [Display(Name = "Purchase Location")]
        public string PurchaseLocation { get; set; } = string.Empty;

        [Display(Name = "Modification Reason")]
        public string ModificationReason { get; set; } = string.Empty;

        public byte[] RecordVersion { get; set; } = Array.Empty<byte>();
    }
}
