using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmploymentStatusDTO
    {
        [Required(ErrorMessage = "Employment Status ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string? Status { get; set; }
        public DateTime? RetirementDate { get; set; }
        public DateTime? TerminationDate { get; set; }
    }
}
