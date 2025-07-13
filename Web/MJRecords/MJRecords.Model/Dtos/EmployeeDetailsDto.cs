using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class EmployeeDetailsDto
    {
        public string? FullName { get; set; }
        public string? Department { get; set; }
        public string? SupervisorFullName { get; set; }
    }

}
