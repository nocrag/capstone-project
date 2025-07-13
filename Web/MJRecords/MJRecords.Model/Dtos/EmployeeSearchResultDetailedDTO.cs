using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmployeeSearchResultDetailedDTO
    {
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Initial")]
        public string? MiddleInitial { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Home Mailing Address")]
        public string? HomeAddress { get; set; }

        [Display(Name = "Work Phone")]
        [DataType(DataType.PhoneNumber)]
        public string? WorkPhone { get; set; }

        [Display(Name = "Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        public string? CellPhone { get; set; }

        [Display(Name = "Work Email Address")]
        public string? WorkEmail { get; set; }

        /*
            123 Main Street
            Apartment 4B
            Moncton, NB E1C 2X3
            Canada

            123 Main Street, Moncton, NB E1C 1A1
         */
    }
}
