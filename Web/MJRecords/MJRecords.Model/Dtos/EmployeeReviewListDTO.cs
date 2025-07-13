using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmployeeReviewListDTO
    {
        public int Id { get; set; }

        [Display(Name = "Employee First Name")]
        public string? EmployeeFName { get; set; }

        [Display(Name = "Employee Last Name")]
        public string? EmployeeLName { get; set; }

        [Display(Name = "Supervisor First Name")]
        public string? SupervisorFName { get; set; }

        [Display(Name = "Supervisor Last Name")]
        public string? SupervisorLName { get; set; }
        public string? Rating { get; set; }
        public string? Comment { get; set; }

        [Display(Name = "Review Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReviewDate { get; set; }

        [Display(Name = "Year Quarter")]
        public string? YearQuarter { get; set; }
    }
}
