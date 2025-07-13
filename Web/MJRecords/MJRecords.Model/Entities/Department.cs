using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class Department : BaseEntity
    {
        [Required(ErrorMessage = "Department ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        [Display(Name = "Department Name")]
        [StringLength(maximumLength: 128, MinimumLength = 3, ErrorMessage = "Department Name minimum length is 3 and maximum length is 128")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Department Description is required")]
        [StringLength(maximumLength: 512, ErrorMessage = "Department description maximum length is 512")]
        public string? Description { get; set; }

        [Display(Name = "Invocation Date")]
        [Required(ErrorMessage = "Department Invocation Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime InvocationDate { get; set; } = DateTime.Now;

        public byte[]? RecordVersion { get; set; }
    }
}
