using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class DepartmentDTO
    {
        [Required(ErrorMessage = "Department ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Department Description is required")]
        public string? Description { get; set; }

        [Display(Name = "Invocation Date")]
        [Required(ErrorMessage = "Department Invocation Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime InvocationDate { get; set; }
    }
}
