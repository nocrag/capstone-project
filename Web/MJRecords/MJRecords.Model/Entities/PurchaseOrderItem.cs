using MJRecords.Model;
using System.ComponentModel.DataAnnotations;

namespace MJRecords.Model
{
    public class PurchaseOrderItem : BaseEntity
    {
        [Key]
        [Display(Name = "Item ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "PO Number")]
        public string PurchaseOrderId { get; set; }

        [Required(ErrorMessage = "Item Name is required.")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 45 characters.")]
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Item Description is required.")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Item Description must be between 5 and 255 characters.")]
        [Display(Name = "Item Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than zero.")]
        public int Quantity { get; set; }


        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 9999999, ErrorMessage = "Price must be greater than zero.")]
        [Display(Name = "Unit Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Justification is required.")]
        [StringLength(255, MinimumLength = 4 , ErrorMessage = "Justification must be between 4 and 255 characters.")]
        public string Justification { get; set; }

        [Required(ErrorMessage = "Purchase Location is required.")]
        [Display(Name = "Purchase Location")]
        [StringLength(255, MinimumLength = 5)]
        public string PurchaseLocation { get; set; }

        [Required(ErrorMessage = "Item Status is required.")]
        [Display(Name = "Item Status")]
        public PurchaseOrderItemStatusEnum Status { get; set; }

        public byte[] RecordVersion { get; set; }

        [Display(Name = "Denial Reason")]
        [StringLength(500)]
        public string? DenialReason { get; set; }

        [Display(Name = "Modification Reason")]
        [StringLength(500)]
        public string? ModificationReason { get; set; }

    }
}
