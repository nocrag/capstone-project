using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmployeeListDTO
    {
        public string Id { get; set; }

        [Display(Name = "Full Name")]
        public string? FullName { get; set; }
        public string? Supervisor { get; set; }
        public string? Department { get; set; }
        public string? Job { get; set; }

        [Display(Name = "Office Location")]
        public string? OfficeLocation { get; set; }

    }
}
