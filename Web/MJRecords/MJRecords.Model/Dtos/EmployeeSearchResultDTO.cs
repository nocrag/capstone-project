using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmployeeSearchResultDTO
    {
        [Display(Name = "Employee")]
        public string? Id { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Work Phone")]
        [DataType(DataType.PhoneNumber)]
        public string? WorkPhone { get; set; }

        [Display(Name = "Office Location")]
        public string? OfficeLocation { get; set; }

        [Display(Name = "Position")]
        public string? Position { get; set; }
    }
}
